using HearMe.BLL.DTM;
using HearMe.BLL.Interfaces;
using HearMe.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic.FileIO;

namespace HearMe.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserToConfirmService _userToConfirmService;
        private readonly IModelService<UserDTM> _userService;
        private readonly IPasswordService _passwordService;
        private readonly IWebHostEnvironment _appEnvironment;

        public AccountController(IUserToConfirmService userToConfirmService, IModelService<UserDTM> userService,
                                 IPasswordService passwordService, IWebHostEnvironment appEnvironment)
        {
            _userToConfirmService = userToConfirmService;
            _userService = userService;
            _passwordService = passwordService;
            _appEnvironment = appEnvironment;
        }

        // GET: AccountController/Login
        [HttpGet]
        public async Task<IActionResult> Login() =>
            !(await _userService.GetItemsList()).Any()
            ? RedirectToAction(nameof(Regist))
            : View();


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!(await _userService.GetItemsList()).Any())
                return RedirectToAction("Regist", "Account");
            if (!ModelState.IsValid)
                return View(model);

            var validUser = (await _userService.GetItemsList()).Where(usr => usr.Login == model.Login).SingleOrDefault();

            if (validUser == null)
            {
                ModelState.AddModelError("", "Incorrect login or password!");
                return View(model);
            }

            UserDTM user = validUser;

            if (!await _passwordService.IsPasswordCorrect(user, model.Password))
            {
                ModelState.AddModelError("", "Incorrect login or password!");
                return View(model);
            }
            CookieOptions option = new()
            {
                Expires = DateTime.Now.AddDays(30)
            };

            HttpContext.Response.Cookies.Append("FullName", user.FirstName + " " + user.LastName, option);
            HttpContext.Response.Cookies.Append("Login", user.Login!, option);
            HttpContext.Response.Cookies.Append("IsAdmin", user.IsAdmin.ToString(), option);
            return RedirectToAction("Index", "Song");
        }

        // GET: AccountController/Regist
        [HttpGet]
        public ActionResult Regist() => View();


        // POST: AccountController/Regist
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Regist([Bind("FirstName, LastName, Login, Password, ConfirmPassword")] RegistVM model, IFormFile? fileUpload)
        {
            if (!ModelState.IsValid)
                return View(model);
            if ((await _userService.GetItemsList()).Any(usr => usr.Login == model.Login))
            {
                ModelState.AddModelError("", "This login is taken!");
                return View(model);
            }
            UserDTM user = new()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Login = model.Login,
                Password = model.Password,
            };
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
                    ModelState.AddModelError("", "Wrong extention! Choose another file!");
                    return View(model);
                }

                string path = "/img/" + fileUpload.FileName;

                using (FileStream filestream = new(_appEnvironment.WebRootPath + path, FileMode.Create))
                    await fileUpload.CopyToAsync(filestream);
                user.AvatarPath = path;
            }
            if (user.Login == "admin")
            {
                user.IsAdmin = true;
                await _userService.CreateItem(user);
            }
            else
                await _userToConfirmService.CreateUserToConfirm(user);

            TempData["SM"] = "Your account has been successfully created. Wait for administrator confirmation";
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            return View(await _userService.GetItem((await _userService.GetItemsList()).Single(usr => usr.Login == HttpContext.Request.Cookies["Login"]).Id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserDTM model, IFormFile? fileUpload)
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
            if (!model.Password.IsNullOrEmpty())
                await _passwordService.ChangePassword(user.Id, model.Password);

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
            return RedirectToAction("Index", "Song");
        }
    }
}
