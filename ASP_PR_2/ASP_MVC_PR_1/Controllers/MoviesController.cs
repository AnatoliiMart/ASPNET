using ASP_MVC_PR_1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP_MVC_PR_1.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MovieContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public MoviesController(MovieContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        // GET: Movies
        public async Task<IActionResult> Index() => View(await _context.Movies.ToListAsync());

        // GET: Movies/Details/Id
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            Movie? movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
                return NotFound();

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create() => View();

        // POST: Movies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MovieName,Director,Genre,Description,RealeaseDate")] Movie movie, IFormFile fileUpload)
        {
            if (ModelState.IsValid && fileUpload.Length > 0)
            {
                if (_context.Movies.Any(_movie => _movie.MovieName == movie.MovieName))
                {
                    ModelState.AddModelError("", "Фильм с таким названием уже сущевствует!");
                    return View(movie);
                }
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
                        ModelState.AddModelError("", "Неверное расширение файла! Выберите другой файл.");
                        return View(movie);
                    }

                    string path = "/img/" + fileUpload.FileName;

                    using (FileStream filestream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                        await fileUpload.CopyToAsync(filestream);
                    movie.PosterPath = path;
                }
                _context.Add(movie);
                await _context.SaveChangesAsync();
                TempData["SM"] = "Фильм был успешно добавлен!";
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Edit/Id
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            Movie? movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                return NotFound();

            return View(movie);
        }

        // POST: Movies/Edit/Id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MovieName,Director,Genre,PosterPath, Description,RealeaseDate")] Movie movie, IFormFile? fileUpload)
        {
            if (id != movie.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(movie);

            try
            {
                if (_context.Movies.Any(_movie => _movie.MovieName == movie.MovieName && _movie.Id != movie.Id))
                {
                    ModelState.AddModelError("", "Фильм с таким названием уже сущевствует!");
                    return View(movie);
                }
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
                        ModelState.AddModelError("", "Неверное расширение файла! Выберите другой файл.");
                        return View(movie);
                    }

                    string path = "/img/" + fileUpload.FileName;

                    using (FileStream filestream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                        await fileUpload.CopyToAsync(filestream);
                    movie.PosterPath = path;
                }
                else
                {
                    IQueryable<string> oldPath = from _movie in _context.Movies
                                                 where _movie.Id == movie.Id
                                                 select _movie.PosterPath;
                    movie.PosterPath = oldPath.First();
                }
                _context.Update(movie);
                await _context.SaveChangesAsync();

                TempData["SM"] = "Фильм был успешно изменен!";

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(movie.Id))
                    return NotFound();
                else
                    throw;
            }
        }

        // GET: Movies/Delete/Id
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            Movie? movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
                return NotFound();

            return View(movie);
        }

        // POST: Movies/Delete/Id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Movie? movie = await _context.Movies.FindAsync(id);

            if (movie != null)
                _context.Movies.Remove(movie);

            await _context.SaveChangesAsync();
            TempData["SM"] = "Фильм был успешно удален!";
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id) => _context.Movies.Any(e => e.Id == id);
    }
}
