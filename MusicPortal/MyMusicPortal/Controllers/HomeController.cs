using Microsoft.AspNetCore.Mvc;
using MyMusicPortal.Models;
using MyMusicPortal.Reposes;
using System.Diagnostics;

namespace MyMusicPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAccountRepository _accountRepository;

        public HomeController(IAccountRepository repository)
        {
            _accountRepository = repository;
        }

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
