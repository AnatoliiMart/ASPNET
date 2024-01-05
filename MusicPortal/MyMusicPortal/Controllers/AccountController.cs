using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyMusicPortal.Models;
using MyMusicPortal.Models.ViewModels;
using MyMusicPortal.Reposes;
using NuGet.Protocol.Core.Types;

namespace MyMusicPortal.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _repository;
        public AccountController(IAccountRepository repository)
        {
            _repository = repository;
        }
        // GET: AccountController
        public async Task<IActionResult> Login()
        {
            if ((await _repository.GetAllUsers()).Count == 0)
                return RedirectToAction("Regist", "Account");

            return View();
        }

        // POST: AccountController/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if ((await _repository.GetAllUsers()).Count == 0)
                return RedirectToAction("Regist", "Account");
            if (!ModelState.IsValid)
                return View(model);

            IQueryable<User> users = _repository.GetUsersByLogin(model.Login);

            if (!users.Any())
            {
                ModelState.AddModelError("", "Incorrect login or password!");
                return View(model);
            }

            User user = users.First();

            if (await _repository.IsPasswordCorrect(user, model.Password))
            {
                ModelState.AddModelError("", "Incorrect login or password!");
                return View(model);
            }
            HttpContext.Session.SetString("FirstName", user.Name ?? string.Empty);
            HttpContext.Session.SetString("LastName", user.Surname ?? string.Empty);
            HttpContext.Session.SetString("Login", user.Login);
            return RedirectToAction("Index", "Home");
        }
        // GET: AccountController/Regist
        public ActionResult Regist() => View();

        // POST: AccountController/Regist
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Regist(RegistVM model)
        {
            if (!ModelState.IsValid)
                return View(model);
            if (await _repository.IsLoginExists(model.Login))
            {
                ModelState.AddModelError("", "This login is taken!");
                return View(model);
            }

            UserToConfirm user = new()
            {
                Name = model.Name,
                Surname = model.Surname,
                Login = model.Login
            };
            user = await _repository.CreateAndHashPassword(user, model.Password);

            await _repository.AddUserOnConfirm(user);

            return RedirectToAction("Login");
        }
    }
}
