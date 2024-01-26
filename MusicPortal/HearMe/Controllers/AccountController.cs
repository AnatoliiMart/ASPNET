using HearMe.BLL.DTM;
using HearMe.BLL.Interfaces;
using HearMe.Models;
using Microsoft.AspNetCore.Mvc;

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
            HttpContext.Session.SetString("FirstName", user.FirstName ?? string.Empty);
            HttpContext.Session.SetString("LastName", user.LastName ?? string.Empty);
            HttpContext.Session.SetString("IsAdmin", user.IsAdmin ? "true" : "false");
            return (HttpContext.Session.GetString("IsAdmin")) == "true" ? RedirectToAction("Index", "Admin") : RedirectToAction("Index", "Home");
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
    }
}
