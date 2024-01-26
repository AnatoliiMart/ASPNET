using HearMe.BLL.DTM;
using HearMe.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HearMe.Controllers
{
    public class GenreController : Controller
    {
        private readonly IModelService<GenreDTM> _genreService;

        public GenreController(IModelService<GenreDTM> genreService)
        {
            _genreService = genreService;
        }

        // GET: GenreController
        public async Task<IActionResult> Index() => View(await _genreService.GetItemsList());

        // GET: GenreController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: GenreController/Create
        public IActionResult CreateGenre() => View(nameof(CreateGenre));

        // POST: GenreController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GenreDTM model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                await _genreService.CreateItem(model);
                TempData["SM"] = "Genre was created sucessfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // GET: GenreController/Edit/5
        public async Task<IActionResult> EditGenre(int id)
        {
            try
            {
                return View(await _genreService.GetItem(id));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: GenreController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditGenre(int id, [Bind("Id, Name")] GenreDTM model)
        {
            if (!ModelState.IsValid)
                return View(model);
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: GenreController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: GenreController/Delete/5
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
