using HearMe.BLL.DTM;
using HearMe.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;

namespace HearMe.Controllers
{
    public class AdminController : Controller
    {
        private readonly IModelService<SongDTM> _songService;
        private readonly IModelService<UserDTM> _userService;
        private readonly IUserToConfirmService _userToConfirmService;
        private readonly IWebHostEnvironment _appEnvironment;

        public AdminController(IModelService<SongDTM> songService, IModelService<UserDTM> userService,
            IUserToConfirmService userToConfirmService, IWebHostEnvironment appEnvironment)
        {
            _songService = songService;
            _userService = userService;
            _userToConfirmService = userToConfirmService;
            _appEnvironment = appEnvironment;
        }

        // GET: AdminController
        public async Task<IActionResult> Index() => View(await _songService.GetItemsList());

        // GET: AdminController/GetUsersToConfirmList
        public async Task<IActionResult> GetUsersToConfirmList() => View(await _userToConfirmService.GetUsersToConfirmList());

        // GET: AdminController/GetUsersList
        public async Task<IActionResult> GetUsersList() => View(await _userService.GetItemsList());

        // GET: AdminController/EditUser/id
        [HttpGet]
        public async Task<IActionResult> EditUser(int id)
        {
            return View(await _userService.GetItem(id));
        }

        // POST: AdminController/EditUser/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(int id, UserDTM model, IFormFile? fileUpload)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Incorrect data");
                return View(model);
            }
            UserDTM user = await _userService.GetItem(id);
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.IsAdmin = model.IsAdmin;
            if (fileUpload != null && fileUpload.Length > 0)
            {
                //Check upload file extension
                string ext = fileUpload.ContentType.ToLower();
                if (ext != "image/jpg" &&
                    ext != "image/jpeg" &&
                    ext != "image/bmp" &&
                    ext != "image/pjpeg" &&
                    ext != "image/gif" &&
                    ext != "image/x-png" &&
                    ext != "image/png")
                {
                    ModelState.AddModelError("", "Incorrect extention! Choose anuther file!");
                    return View(model);
                }

                string path = "/img/" + fileUpload.FileName;
                if (user.AvatarPath != null)
                {
                    string oldPath = user.AvatarPath;
                    FileSystem.DeleteFile(_appEnvironment.WebRootPath + oldPath);
                }
                using (FileStream filestream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    await fileUpload.CopyToAsync(filestream);
                user.AvatarPath = path;
            }
            else
            {
                IEnumerable<string> oldPath = from _movie in await _userService.GetItemsList()
                                              where _movie.Id == model.Id
                                              select _movie.AvatarPath;
                user.AvatarPath = oldPath.First();
            }
            await _userService.UpdateItem(user);
            TempData["SM"] = "User data is refreshed sucessfully";
            return RedirectToAction(nameof(GetUsersList));
        }

        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteItem(id);
                TempData["SM"] = "User was deleted sucessfully";
                return RedirectToAction(nameof(GetUsersList));
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", ex.Message);
                return RedirectToAction(nameof(GetUsersList));
            }
        }

            // POST: AdminController/ConfirmCommonUser/id
            public async Task<IActionResult> ConfirmCommonUser(int id)
        {
            try
            {
                UserDTM? user = await _userToConfirmService.GetUserToConfirm(id);
                await _userService.CreateItem(user);
                await _userToConfirmService.DeleteUserToConfirm(id);
                TempData["SM"] = $"User {user.Login} was confirmed and added to legal users list";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction(nameof(GetUsersToConfirmList));
            }
            return RedirectToAction("Index", "Admin");
        }
        public async Task<IActionResult> ConfirmAdminUser(int id)
        {
            try
            {
                UserDTM? user = await _userToConfirmService.GetUserToConfirm(id);
                user.IsAdmin = true;
                await _userService.CreateItem(user);
                await _userToConfirmService.DeleteUserToConfirm(id);
                TempData["SM"] = $"User {user.Login} was confirmed as ADMIN and added to legal users list";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction(nameof(GetUsersToConfirmList));
            }
            return RedirectToAction("Index", "Admin");
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
