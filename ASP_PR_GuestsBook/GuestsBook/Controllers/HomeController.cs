using GuestsBook.Repos;
using Microsoft.AspNetCore.Mvc;

namespace GuestsBook.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMessagesRepository _repository;

        public HomeController(IMessagesRepository repository) => _repository = repository;

        public async Task<IActionResult> Index() => View(await _repository.GetAllMessages());
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("Login");
            HttpContext.Response.Cookies.Delete("FirstName");
            HttpContext.Response.Cookies.Delete("LastName");
            return RedirectToAction("Index", "Home");
        }
    }
}
