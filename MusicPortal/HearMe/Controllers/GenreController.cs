using HearMe.BLL.DTM;
using HearMe.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HearMe.Controllers
{
    public class GenreController : Controller
    {
        private readonly IModelService<GenreDTM> _genreService;

        public GenreController(IModelService<GenreDTM> genreService) => _genreService = genreService;

        // GET: GenreController
        public async Task<IActionResult> Index() => View(await _genreService.GetItemsList());

        // GET: GenreController/Create
        [HttpGet]
        public IActionResult CreateGenre() => View(nameof(CreateGenre));

        // POST: GenreController/Create
        [HttpPost]
        public async Task<IActionResult> CreateGenre(GenreDTM model)
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
        [HttpGet]
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
            if (id != model.Id)
                return NotFound();
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                _genreService.UpdateItem(model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // GET: GenreController/Delete/5
        public async Task<IActionResult> DeleteGenre(int id)
        {
            try
            {
                await _genreService.DeleteItem(id);
                TempData["SM"] = "Genre was sucessfully deleted";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}
