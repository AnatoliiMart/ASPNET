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
            return View();
        }
    }
}
