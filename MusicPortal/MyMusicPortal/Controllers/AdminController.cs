using Microsoft.AspNetCore.Mvc;
using MyMusicPortal.Models;
using MyMusicPortal.Reposes;

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
        // GET: AdminController/Details/5
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
        // GET: AdminController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(UsersConfirmation));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(UsersConfirmation));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(UsersConfirmation));
            }
            catch
            {
                return View();
            }
        }
    }
}
