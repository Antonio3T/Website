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
    public class PublishersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PublishersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Publisher.ToListAsync());
        }

        public IActionResult Create(string NewName)
        {
            if (ModelState.IsValid)
            {
                var doesPublisherExist = _context.Publisher.Any(p => p.Name == NewName);
                if (doesPublisherExist)
                {
                    ModelState.AddModelError("Publisher", "Publisher already in the database");

                    return PartialView("Listing", _context.Publisher);
                }

                Publisher newPublisher = new Publisher();

                newPublisher.Name = NewName;

                //
                ViewData["publishers"] = _context.Publisher.ToList();
                //

                _context.Publisher.Add(newPublisher);
                _context.SaveChanges();
            }
            //
            ViewData["publishers"] = _context.Publisher.ToList();
            //

            return PartialView("Listing", _context.Publisher);
        }

        public IActionResult Edit(int Id)
        {
            Publisher p = _context.Publisher.SingleOrDefault(p => p.Id == Id);

            return PartialView("Edit", p);
        }

        [HttpPost]
        public string Edit(int id, Publisher p)
        {
            _context.Update(p);
            _context.SaveChanges();

            return p.Name;
        }

        public IActionResult Delete(int Id)
        {
            Publisher p = _context.Publisher.FirstOrDefault(p => p.Id == Id);

            _context.Publisher.Remove(p);
            _context.SaveChanges();

            return PartialView("Listing", _context.Publisher);
        }
    }
}