using Microsoft.AspNetCore.Mvc;
using MyMusicPortal.Models;
using MyMusicPortal.Models.ViewModels;
using MyMusicPortal.Reposes.Account;

namespace MyMusicPortal.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAccountRepository _repository;

        public AdminController(IAccountRepository repository) => _repository = repository;


        // GET: AdminController/UserConfirmation
        public async Task<IActionResult> UsersConfirmation() =>
            View(await _repository.GetAllUsersToConfirm());

        public async Task<IActionResult> ConfirmUser(int id)
        {
            UserToConfirm? confirm = await _repository.GetUserToConfirmById(id);
            if (confirm == null)
            {
                ModelState.AddModelError("", "User not exists!!!");
                return View(nameof(UsersConfirmation));
            }
            User usr = new()
            {
                Name = confirm.Name,
                Surname = confirm.Surname,
                Login = confirm.Login,
                Password = confirm.Password,
                Salt = confirm.Salt
            };
            await _repository.AddConfirmedUser(usr);
            await _repository.RemoveUserFromConfirmationList(confirm);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> DeclineUser(int id)
        {
            UserToConfirm? confirm = await _repository.GetUserToConfirmById(id);
            if (confirm == null)
            {
                ModelState.AddModelError("", "User not exists!!!");
                return View(nameof(UsersConfirmation));
            }
            await _repository.RemoveUserFromConfirmationList(confirm);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult CreateUser() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(RegistVM model)
        {
            if (!ModelState.IsValid)
                return View(model);
            if (await _repository.IsLoginExists(model.Login))
            {
                ModelState.AddModelError("", "This login is taken!");
                return View(model);
            }

            User user = new()
            {
                Name = model.Name,
                Surname = model.Surname,
                Login = model.Login
            };
            user = await _repository.CreateAndHashPassword(user, model.Password);

            await _repository.AddConfirmedUser(user);

            return RedirectToAction(nameof(UsersConfirmation));
        }
    }
}
