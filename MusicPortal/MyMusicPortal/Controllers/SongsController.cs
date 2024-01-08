using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyMusicPortal.Models;
using MyMusicPortal.Reposes.Songs;

namespace MyMusicPortal.Controllers
{
    public class SongsController : Controller
    {
        private readonly MyDbContext _context;
        private readonly ISongsRepository _repository;

        public SongsController(MyDbContext context, ISongsRepository repository)
        {
            _context = context;
            _repository = repository;
        }


        // GET: Songs
        public async Task<IActionResult> Index() => 
            View(await _repository.GetSongsList());

        // GET: Songs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var song = await _repository.GetSongById(id);

            if (song == null)
                return NotFound();

            return View(song);
        }

        // GET: Songs/Create
        public async Task<IActionResult> Create()
        {
            ViewData["GenreId"] = new SelectList(await _repository.GetGenresList(), "Id", "Name");
            ViewData["UserId"] = new SelectList(_repository.GetUserByLogin(HttpContext.Session.GetString("Login")), "Id", "Login");
            return View();
        }

        // POST: Songs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Path,Name,UserId,GenreId")] Song song, IFormFile? fileUpload)
        {
            if (ModelState.IsValid)
            {
                if (await _repository.IsSongUnique(song))
                {
                    ModelState.AddModelError("", "This song is already exists");
                    return View(song);
                }
                if (fileUpload != null && fileUpload.Length > 0)
                {
                    string ext = fileUpload.ContentType.ToLower();
                    if (await _repository.IsExtentionCorrect(ext))
                    {
                        ModelState.AddModelError("", "The extintion is incorrect");
                        return View(song);
                    }
                    await _repository.SongUpload(song, fileUpload);
                }
                await _repository.AddSongToDb(song);
                TempData["SM"] = "Song was added successfully!";
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenreId"] = new SelectList(await _repository.GetGenresList(), "Id", "Name", song.GenreId);
            ViewData["UserId"] = new SelectList(_repository.GetUserByLogin(HttpContext.Session.GetString("Login")), "Id", "Login");
            return View(song);
        }

        // GET: Songs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var song = await _context.Songs.FindAsync(id);
            if (song == null)
            {
                return NotFound();
            }
            ViewData["GenreId"] = new SelectList(await _repository.GetGenresList(), "Id", "Name", song.GenreId);
            ViewData["UserId"] = new SelectList(_repository.GetUserByLogin(HttpContext.Session.GetString("Login")), "Id", "Login");
            return View(song);
        }

        // POST: Songs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Path,Name,UserId,GenreId")] Song song)
        {
            if (id != song.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(song);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SongExists(song.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenreId"] = new SelectList(await _repository.GetGenresList(), "Id", "Name", song.GenreId);
            ViewData["UserId"] = new SelectList(_repository.GetUserByLogin(HttpContext.Session.GetString("Login")), "Id", "Login");
            return View(song);
        }

        // GET: Songs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var song = await _context.Songs
                .Include(s => s.Genre)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (song == null)
            {
                return NotFound();
            }

            return View(song);
        }

        // POST: Songs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var song = await _context.Songs.FindAsync(id);
            if (song != null)
            {
                _context.Songs.Remove(song);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SongExists(int id)
        {
            return _context.Songs.Any(e => e.Id == id);
        }
    }
}
