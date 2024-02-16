using HearMe.BLL.DTM;
using HearMe.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic.FileIO;

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
        [HttpGet]
        public async Task<IActionResult> SelfSongs()
        {
            return View((await _songService.GetItemsList()).Where(item => item.UserLogin == HttpContext.Request.Cookies["Login"]));
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
        public async Task<IActionResult> Create([Bind("Id, Rating, UserId, GenreId")] SongDTM model, IFormFile? previewfile, IFormFile? songfile)
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
                model.Name = Path.GetFileNameWithoutExtension(songfile.FileName);
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
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["GenreId"] = new SelectList(await _genreService.GetItemsList(), "Id", "Name");
            return View(await _songService.GetItem(id));
        }

        // POST: SongController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SongDTM model, IFormFile? fileUpload)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Model is not valid!");
                    return View(model);
                }
                SongDTM song = await _songService.GetItem(id);
                song.Name = model.Name;
                song.GenreName = model.GenreName;
                song.GenreId = model.GenreId;
                if (fileUpload != null && fileUpload.Length > 0)
                {
                    if (await ValidateImageExtention(fileUpload.ContentType.ToLower()))
                    {
                        ModelState.AddModelError("", "The extintion of song preview file is incorrect");
                        return View(model);
                    }
                    string path = "/img/" + fileUpload.FileName;
                    if (song.PreviewPath != null)
                    {
                        string oldPath = song.PreviewPath;
                        FileSystem.DeleteFile(_appEnvironment.WebRootPath + oldPath);
                    }
                    await SongPreviewUpload(song, fileUpload);
                }
                else
                {
                    IEnumerable<string> oldPath = from _song in await _songService.GetItemsList()
                                                  where _song.Id == model.Id
                                                  select _song.PreviewPath;
                    song.PreviewPath = oldPath.First();
                }
                await _songService.UpdateItem(song);
                TempData["SM"] = "Song was edited sucessfully";
                return HttpContext.Request.Cookies["IsAdmin"] == "True"
                       ? RedirectToAction("Index", "Admin")
                       : RedirectToAction(nameof(SelfSongs));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // GET: SongController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _songService.DeleteItem(id);
                TempData["SM"] = "Song was deleted sucessfully";
                return HttpContext.Request.Cookies["IsAdmin"] == "True"
                       ? RedirectToAction("Index", "Admin")
                       : RedirectToAction(nameof(SelfSongs));
            }
            catch (Exception)
            {
                return NotFound();
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
