using HearMe.BLL.DTM;
using HearMe.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HearMe.Controllers
{
    public class AdminController : Controller
    {
        private readonly IModelService<SongDTM> _songService;
        private readonly IModelService<UserDTM> _userService;
        private readonly IUserToConfirmService _userToConfirmService;

        public AdminController(IModelService<SongDTM> songService, IModelService<UserDTM> userService, IUserToConfirmService userToConfirmService)
        {
            _songService = songService;
            _userService = userService;
            _userToConfirmService = userToConfirmService;
        }

        // GET: AdminController
        public async Task<IActionResult> Index() => View(await _songService.GetItemsList());

        // GET: AdminController/GetUsersToConfirmList
        public async Task<IActionResult> GetUsersToConfirmList() => View(await _userToConfirmService.GetUsersToConfirmList());

        // POST: AdminController/ConfirmUser/id
        public async Task<IActionResult> ConfirmUser(int id)
        {
            try
            {
                UserDTM? user = await _userToConfirmService.GetUserToConfirm(id);
                user.IsAdmin = ViewBag.isAdmin;
                await _userService.CreateItem(user);
                await _userToConfirmService.DeleteUserToConfirm(id);
                TempData["SM"] = $"User {user.Login} was confirmed and added to legal users list";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction(nameof(GetUsersToConfirmList));
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: AdminController/DeclineUser/id
        public async Task<IActionResult> DeclineUser(int id)
        {
            try
            {
                await _userToConfirmService.DeleteUserToConfirm(id);
                TempData["SM"] = "User was declined";
                return RedirectToAction(nameof(GetUsersToConfirmList));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction(nameof(GetUsersToConfirmList));
            }
        }
    }
}
