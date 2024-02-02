using HearMe.BLL.DTM;
using HearMe.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HearMe.Controllers
{
    public class SongController : Controller
    {
        private readonly IModelService<SongDTM> _songService;
        private readonly IModelService<GenreDTM> _genreService;
        private readonly IModelService<UserDTM> _userService;
        private readonly IWebHostEnvironment _appEnvironment;

        public SongController(IModelService<SongDTM> songService, IModelService<GenreDTM> genreService,
                              IWebHostEnvironment appEnvironment, IModelService<UserDTM> userService)
        {
            _songService = songService;
            _genreService = genreService;
            _appEnvironment = appEnvironment;
            _userService = userService;
        }



        // GET: SongController
        public async Task<IActionResult> Index()
        {
            return View(await _songService.GetItemsList());
        }

        // GET: SongController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SongController/Create
        public async Task<IActionResult> Create()
        {
            ViewData["GenreId"] = new SelectList(await _genreService.GetItemsList(), "Id", "Name");
            ViewData["UserId"] = (await _userService.GetItem((await _userService.GetItemsList()).Single(usr => usr.Login == HttpContext.Request.Cookies["Login"]).Id)).Id;
            return View();
        }

        // POST: SongController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Name, Rating, UserId, GenreId")] SongDTM model, IFormFile? previewfile, IFormFile? songfile)
        {
            if (!ModelState.IsValid)
                return View(model);
            if ((await _songService.GetItemsList()).Any(sng => sng.Name == model.Name))
            {
                ModelState.AddModelError("", "Song with this name already exists");
                return View(model);
            }
            try
            {
                if (previewfile == null || previewfile.Length == 0)
                    throw new Exception("Preview file does not exists");
                if (songfile == null || songfile.Length == 0)
                    throw new Exception("File does not exists");

                if (await ValidateImageExtention(previewfile.ContentType.ToLower()))
                {
                    ModelState.AddModelError("", "The extintion of song preview file is incorrect");
                    return View(model);
                }
                if (await ValidateSongExtention(songfile.ContentType.ToLower()))
                {
                    ModelState.AddModelError("", "The extintion of song audio file is incorrect");
                    return View(model);
                }
                await SongAudioUpload(model, songfile);
                await SongPreviewUpload(model, previewfile);
                await _songService.CreateItem(model);
                TempData["SM"] = "Song was added successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // GET: SongController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SongController/Edit/5
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

        // GET: SongController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SongController/Delete/5
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
        private async Task<bool> ValidateImageExtention(string extention) =>
            await Task.Run(() =>
                (extention != "image/jpg" &&
                 extention != "image/jpeg" &&
                 extention != "image/bmp" &&
                 extention != "image/pjpeg" &&
                 extention != "image/gif" &&
                 extention != "image/x-png" &&
                 extention != "image/png"));

        private async Task<bool> ValidateSongExtention(string extention) =>
            await Task.Run(() =>
                   (extention != "audio/flac" &&
                    extention != "audio/mpeg" &&
                    extention != "audio/wav" &&
                    extention != "audio/aac" &&
                    extention != "audio/m4a" &&
                    extention != "audio/ogg" &&
                    extention != "audio/mid"));

        public async Task SongPreviewUpload(SongDTM song, IFormFile fileUpload)
        {
            string path = "/img/" + fileUpload.FileName;

            using (FileStream filestream = new(_appEnvironment.WebRootPath + path, FileMode.Create))
                await fileUpload.CopyToAsync(filestream);
            song.PreviewPath = path;
        }

        public async Task SongAudioUpload(SongDTM song, IFormFile fileUpload)
        {
            string path = "/audio/" + fileUpload.FileName;

            using (FileStream filestream = new(_appEnvironment.WebRootPath + path, FileMode.Create))
                await fileUpload.CopyToAsync(filestream);
            song.SongPath = path;
        }


    }
}
