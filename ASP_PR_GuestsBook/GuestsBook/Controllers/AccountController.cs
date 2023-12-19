using GuestsBook.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace GuestsBook.Controllers
{
    public class AccountController : Controller
    {
        private readonly MyDbContext _context;
        public AccountController(MyDbContext context) => _context = context;
        // GET: AccountController
        public ActionResult Login()
        {
            if (_context.Users.ToList().Count == 0)
                return RedirectToAction("Regist", "Account");

            return View();
        }

        // POST: AccountController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginMDL model)
        { 
            if (_context.Users.ToList().Count == 0)
                return RedirectToAction("Regist", "Account");
            if (!ModelState.IsValid)
                return View(model);

            IQueryable<User> users = _context.Users.Where(x => x.Login == model.Login);

            if (!users.Any())
            {
                ModelState.AddModelError("", "Incorrect login or password!");
                return View(model);
            }
            User user = users.First();
            string salt = user.Salt;
            byte[] password = Encoding.Unicode.GetBytes(salt + model.Password);

            var sha256 = SHA256.Create();
            byte[] hashPassword = sha256.ComputeHash(password);

            StringBuilder hash = new StringBuilder(hashPassword.Length);
            for (int i = 0; i < hashPassword.Length; i++)
                hash.Append(string.Format("{0:X2}", hashPassword[i]));

            if (user.Password != hash.ToString())
            {
                ModelState.AddModelError("", "Incorrect login or password!");
                return View(model);
            }
            HttpContext.Session.SetString("FirstName", user.FirstName ?? string.Empty);
            HttpContext.Session.SetString("LastName", user.LastName ?? string.Empty);
            return RedirectToAction("Index", "Home");
        }

        // GET: AccountController/Regist
        [HttpGet]
        public ActionResult Regist() => View();

        // POST: AccountController/Regist
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Regist(RegistMDL model)
        {
            if (!ModelState.IsValid)
                return View(model);

            User user = new()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Login = model.Login
            };

            byte[] saltbuf = new byte[16];

            RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(saltbuf);

            StringBuilder sb = new StringBuilder(16);
            for (int i = 0; i < 16; i++)
                sb.Append(string.Format("{0:X2}", saltbuf[i]));

            string salt = sb.ToString();
            byte[] password = Encoding.Unicode.GetBytes(salt + model.Password);

            var sha256 = SHA256.Create();
            byte[] hashPassword = sha256.ComputeHash(password);

            StringBuilder hash = new StringBuilder(hashPassword.Length);
            for (int i = 0; i < hashPassword.Length; i++)
                hash.Append(string.Format("{0:X2}", hashPassword[i]));

            user.Password = hash.ToString();
            user.Salt = salt;
            _context.Users.Add(user);
            _context.SaveChanges();
            return RedirectToAction("Login");
        }
    }
}
