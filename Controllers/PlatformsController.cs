using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website.Data;
using Website.Models;

namespace Website.Controllers
{
    //
    [Authorize(Roles = "Admin, Employee")]
    //
    public class PlatformsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlatformsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Platforms
        public async Task<IActionResult> Index()
        {
            return View(await _context.Platform.ToListAsync());
        }

        public IActionResult Create(string NewName)
        {
            if (ModelState.IsValid)
            {
                var doesPlatformExist = _context.Platform.Any(p => p.Name == NewName);
                if (doesPlatformExist)
                {
                    ModelState.AddModelError("Platform", "Platform already in the database");

                    return PartialView("Listing", _context.Platform);
                }

                Platform newPlatform = new Platform();

                newPlatform.Name = NewName;

                //
                ViewData["platforms"] = _context.Platform.ToList();
                //

                _context.Platform.Add(newPlatform);
                _context.SaveChanges();
            }
            //
            ViewData["platforms"] = _context.Platform.ToList();
            //

            return PartialView("Listing", _context.Platform);
        }

        public IActionResult Edit(int Id)
        {
            Platform p = _context.Platform.SingleOrDefault(p => p.Id == Id);

            return PartialView("Edit", p);
        }

        [HttpPost]
        public string Edit(int id, Platform p)
        {
            _context.Update(p);
            _context.SaveChanges();

            return p.Name;
        }

        public IActionResult Delete(int Id)
        {
            Platform p = _context.Platform.FirstOrDefault(p => p.Id == Id);

            _context.Platform.Remove(p);
            _context.SaveChanges();

            return PartialView("Listing", _context.Platform);
        }
    }
}