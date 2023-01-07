using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Website.Data;
using Website.Models;

namespace Website.Controllers
{
    public class FavoriteCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FavoriteCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            //var applicationDbContext = _context.FavoriteCategories.Include(f => f.Category).Include(f => f.Profile);
            var applicationDbContext = _context.FavoriteCategories.Include(p => p.Category).Include(p => p.Profile).Where(p => p.Profile.Username.Equals(User.Identity.Name));

            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FavoriteCategories == null)
            {
                return NotFound();
            }

            var favoriteCategories = await _context.FavoriteCategories
                .Include(f => f.Category)
                .Include(f => f.Profile)
                .FirstOrDefaultAsync(m => m.CategoryID == id);
            if (favoriteCategories == null)
            {
                return NotFound();
            }

            return View(favoriteCategories);
        }

        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Category, "Id", "Name");
            ViewData["ProfileID"] = new SelectList(_context.Profiles, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryID,ProfileID")] FavoriteCategories favoriteCategories)
        {
            if (ModelState.IsValid)
            {
                _context.Add(favoriteCategories);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.Category, "Id", "Name", favoriteCategories.CategoryID);
            ViewData["ProfileID"] = new SelectList(_context.Profiles, "Id", "Id", favoriteCategories.ProfileID);
            return View(favoriteCategories);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FavoriteCategories == null)
            {
                return NotFound();
            }

            var favoriteCategories = await _context.FavoriteCategories.FindAsync(id);
            if (favoriteCategories == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Category, "Id", "Name", favoriteCategories.CategoryID);
            ViewData["ProfileID"] = new SelectList(_context.Profiles, "Id", "Id", favoriteCategories.ProfileID);
            return View(favoriteCategories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryID,ProfileID")] FavoriteCategories favoriteCategories)
        {
            if (id != favoriteCategories.CategoryID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(favoriteCategories);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FavoriteCategoriesExists(favoriteCategories.CategoryID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.Category, "Id", "Name", favoriteCategories.CategoryID);
            ViewData["ProfileID"] = new SelectList(_context.Profiles, "Id", "Id", favoriteCategories.ProfileID);
            return View(favoriteCategories);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FavoriteCategories == null)
            {
                return NotFound();
            }

            var favoriteCategories = await _context.FavoriteCategories
                .Include(f => f.Category)
                .Include(f => f.Profile)
                .FirstOrDefaultAsync(m => m.CategoryID == id);
            if (favoriteCategories == null)
            {
                return NotFound();
            }

            return View(favoriteCategories);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FavoriteCategories == null)
            {
                return Problem("Entity set 'ApplicationDbContext.FavoriteCategories'  is null.");
            }
            var favoriteCategories = await _context.FavoriteCategories.FindAsync(id, _context.Profiles.SingleOrDefault(x => x.Username == User.Identity.Name).Id);
            if (favoriteCategories != null)
            {
                _context.FavoriteCategories.Remove(favoriteCategories);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FavoriteCategoriesExists(int id)
        {
            return _context.FavoriteCategories.Any(e => e.CategoryID == id);
        }
    }
}