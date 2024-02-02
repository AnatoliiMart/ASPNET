using GuestsBook.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GuestsBook.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Request.Cookies["LastName"] != null
                   && HttpContext.Request.Cookies["FirstName"] != null)                
                return View();
            else
                return RedirectToAction("Login", "Account");
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("Login");
            HttpContext.Response.Cookies.Delete("FirstName");
            HttpContext.Response.Cookies.Delete("LastName");
            return RedirectToAction("Login", "Account");
        }
    }
}
