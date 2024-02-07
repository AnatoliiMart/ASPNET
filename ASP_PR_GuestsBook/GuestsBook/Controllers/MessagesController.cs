using GuestsBook.Models;
using GuestsBook.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GuestsBook.Controllers
{
    public class MessagesController : Controller
    {
        private readonly IMessagesRepository _repository;
        public MessagesController(IMessagesRepository repository) => _repository = repository;

        // GET: Messages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return Problem("Message was not found!");

            Message? msg = await _repository.GetMessageDetails(id);

            return (msg == null)
                   ? Problem("Message was not found!")
                   : PartialView("Details", msg);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_repository.GetUserByLogin(HttpContext.Request.Cookies["Login"]), "Id", "Login");
            return PartialView();
        }

        // POST: Messages/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Mesage,UserId")] Message message)
        {
            if (ModelState.IsValid)
            {
                await _repository.CreateMessage(message);
                await _repository.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            ViewData["UserId"] = new SelectList(_repository.GetUserByLogin(HttpContext.Request.Cookies["Login"]), "Id", "Login", message.UserId);
            return Problem("Add message error! Try again!");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return Problem("NotFound!");
            var model = await _repository.GetMessage(id);
            if (model == null) return Problem("NotFound");
            ViewData["UserId"] = new SelectList(_repository.GetUserByLogin(HttpContext.Request.Cookies["Login"]), "Id", "Login", model.UserId);
            return PartialView(model);
        }

        // POST: Messages/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit([Bind("Id,Mesage,UserId")] Message message)
        {

            if (ModelState.IsValid)
            {
                _repository.UpdateMessage(message);
                await _repository.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return Problem("Incorrect data");
        }


        // POST: Messages/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if ((await _repository.GetAllMessages()).Count == 0)
                return Problem("The messages list is empty! Nothing to delete!");
            await _repository.DeleteMessage(id);
            await _repository.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}
