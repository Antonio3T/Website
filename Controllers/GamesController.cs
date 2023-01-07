using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Website.Data;
using Website.Filters;
using Website.Models;

namespace Website.Controllers
{
    //
    //[Authorize(Roles = "Admin, Employee")]
    //
    //
    //[DailyMaintenanceFilter(From = new[] { 22, 30, 0 }, To = new[] { 23, 59, 59 })]
    [DailyMaintenanceFilter(From = new[] { 23, 8, 0 }, To = new[] { 23, 9, 0 })]
    //
    public class GamesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GamesController(ApplicationDbContext context)
        {
            _context = context;
        }

        //public async Task<IActionResult> Index()
        //{
        //    //
        //    var ApplicationDbContext = _context.PurchaseReceipts.Include(x => x.Game);
        //    //
        //    //return View(await _context.Game.ToListAsync());
        //    return View(await _context.Game.OrderBy(alphabetically => alphabetically.Name).ToListAsync());
        //}

        //
        public async Task<IActionResult> Index(string gameGenre, string searchString)
        {
            //
            var ApplicationDbContext = _context.PurchaseReceipts.Include(x => x.Game);
            //

            if (_context.Game == null)
            {
                return Problem("Entity set 'Game'  is null.");
            }

            IQueryable<string> genreQuery = from g in _context.Game
                                            orderby g.Category
                                            select g.Category;

            var games = from g in _context.Game
                        select g;

            if (!String.IsNullOrEmpty(searchString))
            {
                games = games.Where(n => n.Name.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(gameGenre))
            {
                games = games.Where(x => x.Category == gameGenre);
            }

            var gameGenreVM = new GameGenreViewModel
            {
                Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
                Games = await games.OrderBy(alphabetically => alphabetically.Name).ToListAsync()
            };

            //return View(await _context.Game.ToListAsync());
            //return View(await _context.Game.OrderBy(alphabetically => alphabetically.Name).ToListAsync());
            //return View(await games.OrderBy(alphabetically => alphabetically.Name).ToListAsync());

            return View(gameGenreVM);
        }
        //

        //
        [Authorize]
        public IActionResult Purchase(int id)
        {
            var game = _context.Games.Find(id);

            return View(game);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ConfirmPurchase(int id)
        {
            var doesReceiptExist = _context.PurchaseReceipts.Any(p => p.ProfileID == _context.Profiles.SingleOrDefault(x => x.Username == User.Identity.Name).Id && p.GameID == id);
            
            if (doesReceiptExist)
            {
                //ModelState.AddModelError("GameID", "Receipt already added");
                return RedirectToAction(nameof(Index));
            }

            Game game = _context.Games.Find(id);

            float price = game.Price;

            await _context.Game.FirstOrDefaultAsync(x => x.Id == game.Id);


            int ProfileID = _context.Profiles.SingleOrDefault(x => x.Username == User.Identity.Name).Id;


            var purchase = new PurchaseReceipts();
            purchase.ProfileID = ProfileID;
            purchase.GameID = id;
            purchase.Value = price;

            _context.PurchaseReceipts.Add(purchase);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        //

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Game == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        //
        [Authorize(Roles = "Admin, Employee")]
        //
        public IActionResult Create()
        {
            //
            ViewData["categories"] = _context.Category.ToList();
            ViewData["platforms"] = _context.Platform.ToList();
            ViewData["publishers"] = _context.Publisher.ToList();
            //

            return View();
        }

        //
        [Authorize(Roles = "Admin, Employee")]
        //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Picture,ReleaseDate,Description,Category,Platform,Publisher")] Game game)
        {
            if (ModelState.IsValid)
            {
                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(game);
        }

        //
        [Authorize(Roles = "Admin, Employee")]
        //
        public async Task<IActionResult> Edit(int? id)
        {
            //
            ViewData["categories"] = _context.Category.ToList();
            ViewData["platforms"] = _context.Platform.ToList();
            ViewData["publishers"] = _context.Publisher.ToList();
            //

            if (id == null || _context.Game == null)
            {
                return NotFound();
            }

            var game = await _context.Game.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            return View(game);
        }

        //
        [Authorize(Roles = "Admin, Employee")]
        //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Picture,ReleaseDate,Description,Category,Platform,Publisher")] Game game)
        {
            if (id != game.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.Id))
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
            return View(game);
        }

        //
        [Authorize(Roles = "Admin, Employee")]
        //
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Game == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        //
        [Authorize(Roles = "Admin, Employee")]
        //
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Game == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Game'  is null.");
            }
            var game = await _context.Game.FindAsync(id);
            if (game != null)
            {
                _context.Game.Remove(game);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //
        [Authorize(Roles = "Admin, Employee")]
        //
        private bool GameExists(int id)
        {
            return _context.Game.Any(e => e.Id == id);
        }
    }
}