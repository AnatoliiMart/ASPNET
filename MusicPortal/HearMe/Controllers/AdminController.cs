using HearMe.BLL.DTM;
using HearMe.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HearMe.Controllers
{
    public class AdminController : Controller
    {
        private readonly IModelService<SongDTM> _songService;
        private readonly IModelService<GenreDTM> _genreService;
        private readonly IModelService<UserDTM> _userService;
        private readonly IUserToConfirmService _userToConfirmService;

        public AdminController(IModelService<SongDTM> songService, IModelService<GenreDTM> genreService,
                               IModelService<UserDTM> userService, IUserToConfirmService userToConfirmService)
        {
            _songService = songService;
            _genreService = genreService;
            _userService = userService;
            _userToConfirmService = userToConfirmService;
        }

        // GET: AdminController
        public async Task<IActionResult> Index() => View(await _songService.GetItemsList());

        // GET: AdminController/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
                return RedirectToAction(nameof(Index));
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
                return RedirectToAction(nameof(Index));
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
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
