using Microsoft.AspNetCore.Mvc;

namespace HearMe.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("LastName") != null
                   && HttpContext.Session.GetString("FirstName") != null)
                return View();
            else
                return RedirectToAction("Login", "Account");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}
