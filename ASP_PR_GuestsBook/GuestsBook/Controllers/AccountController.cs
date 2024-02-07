using GuestsBook.Models;
using GuestsBook.Repos;
using Microsoft.AspNetCore.Mvc;

namespace GuestsBook.Controllers
{
    public class AccountController : Controller
    {

        private readonly IAccountsRepository _repository;
        public AccountController(IAccountsRepository repository)
        {
            _repository = repository;
        }

        // GET: AccountController/Login
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            if ((await _repository.GetAllUsers()).Count == 0)
                return RedirectToAction("Regist", "Account");

            return PartialView(nameof(Login));
        }

        // POST: AccountController/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginMDL model)
        {
            if ((await _repository.GetAllUsers()).Count == 0)
                return RedirectToAction("Regist", "Account");
            if (!ModelState.IsValid)
                return PartialView(model);

            IQueryable<User> users = _repository.GetUsersByLogin(model);

            if (!users.Any())
            {
                ModelState.AddModelError("", "Incorrect login or password!");
                return PartialView(model);
            }

            User user = users.First();

            if (await _repository.IsPasswordCorrect(user, model))
            {
                ModelState.AddModelError("", "Incorrect login or password!");
                return PartialView(model);
            }
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddDays(30);
            HttpContext.Response.Cookies.Append("FirstName", user.FirstName ?? string.Empty, options);
            HttpContext.Response.Cookies.Append("LastName", user.LastName ?? string.Empty, options);
            HttpContext.Response.Cookies.Append("Login", user.Login, options);
            return RedirectToAction("Index", "Home");
        }

        // GET: AccountController/Regist
        [HttpGet]
        public ActionResult Regist() => PartialView();

        // POST: AccountController/Regist
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Regist(RegistMDL model)
        {
            if (!ModelState.IsValid)
                return PartialView(model);

            if (await _repository.IsLoginExists(model.Login))
            {
                ModelState.AddModelError("", "This login is taken!");
                return PartialView(model);
            }

            User user = new()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Login = model.Login
            };
            user = await _repository.CreateAndHashPassword(user, model);

            await _repository.AddUserToDb(user);

            return RedirectToAction("Index", "Home");
        }
    }
}
