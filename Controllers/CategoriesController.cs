using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website.Data;
using Website.Models;

namespace Website.Controllers
{
    //
    //[Authorize(Roles = "Admin, Employee")]
    //
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        //
        [Authorize]
        //
        public async Task<IActionResult> Index()
        {
            return View(await _context.Category.ToListAsync());
        }

        //
        [Authorize]
        public IActionResult AddFavoriteCategory(int id)
        {
            var category = _context.Category.Find(id);

            return View(category);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ConfirmCategory(int id)
        {
            var doesFavoriteCategoryExist = _context.FavoriteCategories.Any(c => c.ProfileID == _context.Profiles.SingleOrDefault(x => x.Username == User.Identity.Name).Id && c.CategoryID == id);

            if (doesFavoriteCategoryExist)
            {
                ModelState.AddModelError("CategoryID", "Category already added");
                return RedirectToAction(nameof(Index));
            }

            Category category = _context.Category.Find(id);


            await _context.Game.FirstOrDefaultAsync(x => x.Id == category.Id);


            int ProfileID = _context.Profiles.SingleOrDefault(x => x.Username == User.Identity.Name).Id;


            var favoritecategory = new FavoriteCategories();
            favoritecategory.ProfileID = ProfileID;
            favoritecategory.CategoryID = id;

            _context.FavoriteCategories.Add(favoritecategory);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        //

        //
        [Authorize(Roles = "Admin, Employee")]
        //
        public IActionResult Create(string NewName)
        {
            if (ModelState.IsValid)
            {
                var doesCategoryExist = _context.Category.Any(c => c.Name == NewName);
                if (doesCategoryExist)
                {
                    ModelState.AddModelError("Category", "Category already in the database");

                    return PartialView("Listing", _context.Category);
                }

                Category newCategory = new Category();
                newCategory.Name = NewName;

                //
                ViewData["categories"] = _context.Category.ToList();
                //

                _context.Category.Add(newCategory);
                _context.SaveChanges();
            }
            //
            ViewData["categories"] = _context.Category.ToList();
            //

            return PartialView("Listing", _context.Category);
        }

        //
        [Authorize(Roles = "Admin, Employee")]
        //
        public IActionResult Edit(int Id)
        {
            Category c = _context.Category.SingleOrDefault(c => c.Id == Id);

            return PartialView("Edit", c);
        }

        //
        [Authorize(Roles = "Admin, Employee")]
        //
        [HttpPost]
        public string Edit(int id, Category c)
        {
            _context.Update(c);
            _context.SaveChanges();

            return c.Name;
        }

        //
        [Authorize(Roles = "Admin, Employee")]
        //
        public IActionResult Delete(int Id)
        {
            Category c = _context.Category.FirstOrDefault(c => c.Id == Id);

            _context.Category.Remove(c);
            _context.SaveChanges();

            return PartialView("Listing", _context.Category);
        }
    }
}