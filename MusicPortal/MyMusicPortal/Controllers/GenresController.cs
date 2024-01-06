using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMusicPortal.Models;
using MyMusicPortal.Reposes.Genres;

namespace MyMusicPortal.Controllers
{
    public class GenresController : Controller
    {
        private readonly IGenresRepository _repository;

        public GenresController(IGenresRepository repository) => _repository = repository;

        // GET: Genres
        public async Task<IActionResult> Index() => View(await _repository.GetGenresList());

        // GET: Genres/Create
        public IActionResult Create() => View();

        // POST: Genres/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Genre genre)
        {
            if (ModelState.IsValid)
            {
                await _repository.AddGenre(genre);
                return RedirectToAction(nameof(Index));
            }
            return View(genre);
        }

        // GET: Genres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var genre = await _repository.GetGenre(id);

            if (genre == null)
                return NotFound();

            return View(genre);
        }

        // POST: Genres/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Genre genre)
        {
            if (id != genre.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                   await _repository.UpdateGenre(genre);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _repository.GenreExists(id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(genre);
        }

        // GET: Genres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var genre = await _repository.GetGenreByFoD(id);
            if (genre == null)
                return NotFound();

            return View(genre);
        }

        // POST: Genres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var genre = await _repository.GetGenre(id);
            if (genre != null)
            {
                await _repository.DeleteGenre(genre);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
