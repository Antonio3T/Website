using Microsoft.AspNetCore.Mvc;

namespace Website.Filters
{
    public class SecurityController : Controller
    {
        public IActionResult Maintenance()
        {
            HttpContext.Response.StatusCode = 405;

            return View();
        }
    }
}