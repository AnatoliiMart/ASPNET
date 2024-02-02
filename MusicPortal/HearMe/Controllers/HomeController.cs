using Microsoft.AspNetCore.Mvc;

namespace HearMe.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return HttpContext.Request.Cookies["Login"] != null
                   ? View()
                   : RedirectToAction("Login", "Account");
        }

        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("FullName");
            HttpContext.Response.Cookies.Delete("Login");
            HttpContext.Response.Cookies.Delete("IsAdmin");

            return RedirectToAction("Login", "Account");
        }
    }
}
