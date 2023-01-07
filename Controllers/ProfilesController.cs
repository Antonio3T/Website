using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website.Data;
using Website.Models;

namespace Website.Controllers
{
    //
    [Authorize(Roles = "Admin")]
    //
    public class ProfilesController : Controller
    {
        //private readonly ApplicationDbContext _context;

        //public ProfilesController(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        //
        private readonly ApplicationDbContext _dbcontext;
        private readonly RoleManager<IdentityRole> _roleManager;
        //

        public ProfilesController(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            //
            ApplicationDbContext dbcontext,
            RoleManager<IdentityRole> roleManager
            //
            )
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            //
            _dbcontext = dbcontext;
            _roleManager = roleManager;
            //
        }

        //
        public async Task<IActionResult> CreateEmployeeProfile()
        {
            //
            //ViewData["roles"] = _roleManager.Roles.ToList();
            ViewData["roles"] = _roleManager.Roles.Where(r => r.Name != "Client").ToList();
            //

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployeeProfile(EmployeeProfile newprofile)
        {
            if (ModelState.IsValid)
            {
                var doesUserNameExist = _dbcontext.Users.Any(u => u.UserName == newprofile.Username);
                var doesEmailExist = _dbcontext.Users.Any(e => e.Email == newprofile.Email);

                if (doesUserNameExist)
                {
                    ModelState.AddModelError("UserName", "UserName already taken");
                    return RedirectToAction("Index");
                }

                if (doesEmailExist)
                {
                    ModelState.AddModelError("Email", "Email already taken");
                    return RedirectToAction("Index");
                }

                var profile = CreateUser();

                await _userStore.SetUserNameAsync(profile, newprofile.Username, CancellationToken.None);
                await _emailStore.SetEmailAsync(profile, newprofile.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(profile, newprofile.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    await _userManager.AddToRoleAsync(profile, newprofile.Role);

                    Profile nprofile = new Profile { Username = profile.UserName, Role = newprofile.Role };

                    _dbcontext.Profiles.Add(nprofile);
                    await _dbcontext.SaveChangesAsync();
                }
                else
                {
                    //
                    ViewData["roles"] = _roleManager.Roles.Where(r => r.Name != "Client").ToList();

                    return View(newprofile);
                    //
                }
            }
            //
            ViewData["roles"] = _roleManager.Roles.Where(r => r.Name != "Client").ToList();
            //

            return RedirectToAction("Index");
        }
        //

        public async Task<IActionResult> Index()
        {
            return View(await _dbcontext.Profiles.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _dbcontext.Profiles == null)
            {
                return NotFound();
            }

            var profile = await _dbcontext.Profiles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (profile == null)
            {
                return NotFound();
            }

            return View(profile);
        }

        public IActionResult Create()
        {
            //
            ViewData["roles"] = _roleManager.Roles.Where(r => r.Name != "Client").ToList();
            //

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,Birthday,Role")] Profile profile)
        {
            if (ModelState.IsValid)
            {
                _dbcontext.Add(profile);
                await _dbcontext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(profile);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _dbcontext.Profiles == null)
            {
                return NotFound();
            }

            var profile = await _dbcontext.Profiles.FindAsync(id);
            if (profile == null)
            {
                return NotFound();
            }
            return View(profile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Birthday,Role")] Profile profile)
        {
            if (id != profile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbcontext.Profiles.Update(profile);
                    await _dbcontext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfileExists(profile.Id))
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
            return View(profile);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _dbcontext.Profiles == null)
            {
                return NotFound();
            }

            var profile = await _dbcontext.Profiles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (profile == null)
            {
                return NotFound();
            }

            return View(profile);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_dbcontext.Profiles == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Profiles'  is null.");
            }
            var profile = await _dbcontext.Profiles.FindAsync(id);
            if (profile != null)
            {
                _dbcontext.Profiles.Remove(profile);
            }

            await _dbcontext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfileExists(int id)
        {
            return _dbcontext.Profiles.Any(e => e.Id == id);
        }

        //
        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
        //
    }
}