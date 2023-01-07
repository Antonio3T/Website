using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Website.Data;
using Website.Models;

namespace Website.Controllers
{
    public class PurchaseReceiptsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PurchaseReceiptsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            //var applicationDbContext = _context.PurchaseReceipts.Include(p => p.Game).Include(p => p.Profile);
            var applicationDbContext = _context.PurchaseReceipts.Include(p => p.Game).Include(p => p.Profile).Where(p => p.Profile.Username.Equals(User.Identity.Name));
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PurchaseReceipts == null)
            {
                return NotFound();
            }

            var purchaseReceipts = await _context.PurchaseReceipts
                .Include(p => p.Game)
                .Include(p => p.Profile)
                .FirstOrDefaultAsync(m => m.GameID == id);
            if (purchaseReceipts == null)
            {
                return NotFound();
            }

            return View(purchaseReceipts);
        }

        public IActionResult Create()
        {
            ViewData["GameID"] = new SelectList(_context.Game, "Id", "Category");
            ViewData["ProfileID"] = new SelectList(_context.Profiles, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GameID,ProfileID")] PurchaseReceipts purchaseReceipts)
        {
            if (ModelState.IsValid)
            {
                _context.Add(purchaseReceipts);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GameID"] = new SelectList(_context.Game, "Id", "Category", purchaseReceipts.GameID);
            ViewData["ProfileID"] = new SelectList(_context.Profiles, "Id", "Id", purchaseReceipts.ProfileID);
            return View(purchaseReceipts);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PurchaseReceipts == null)
            {
                return NotFound();
            }

            var purchaseReceipts = await _context.PurchaseReceipts.FindAsync(id);
            if (purchaseReceipts == null)
            {
                return NotFound();
            }
            ViewData["GameID"] = new SelectList(_context.Game, "Id", "Category", purchaseReceipts.GameID);
            ViewData["ProfileID"] = new SelectList(_context.Profiles, "Id", "Id", purchaseReceipts.ProfileID);
            return View(purchaseReceipts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GameID,ProfileID")] PurchaseReceipts purchaseReceipts)
        {
            if (id != purchaseReceipts.GameID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(purchaseReceipts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchaseReceiptsExists(purchaseReceipts.GameID))
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
            ViewData["GameID"] = new SelectList(_context.Game, "Id", "Category", purchaseReceipts.GameID);
            ViewData["ProfileID"] = new SelectList(_context.Profiles, "Id", "Id", purchaseReceipts.ProfileID);
            return View(purchaseReceipts);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PurchaseReceipts == null)
            {
                return NotFound();
            }

            var purchaseReceipts = await _context.PurchaseReceipts
                .Include(p => p.Game)
                .Include(p => p.Profile)
                .FirstOrDefaultAsync(m => m.GameID == id);
            if (purchaseReceipts == null)
            {
                return NotFound();
            }

            return View(purchaseReceipts);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PurchaseReceipts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.PurchaseReceipts'  is null.");
            }
            var purchaseReceipts = await _context.PurchaseReceipts.FindAsync(id);
            if (purchaseReceipts != null)
            {
                _context.PurchaseReceipts.Remove(purchaseReceipts);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchaseReceiptsExists(int id)
        {
            return _context.PurchaseReceipts.Any(e => e.GameID == id);
        }
    }
}