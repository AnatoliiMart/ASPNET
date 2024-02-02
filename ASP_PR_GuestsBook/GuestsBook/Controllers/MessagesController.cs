using GuestsBook.Models;
using GuestsBook.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace GuestsBook.Controllers
{
    public class MessagesController : Controller
    {
        private readonly IMessagesRepository _repository;
        public MessagesController(IMessagesRepository repository) => _repository = repository;

        // GET: Messages
        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> GetAllMessages()
        {
            return ((await _repository.GetAllMessages()).Count == 0)
                   ? Problem("No Messages")
                   : Json((await _repository.GetAllMessages()).ToJson());
        }

        public async Task<IActionResult> MyMessages()
        {
            User userSpecification = await _repository.GetUserByLogin(HttpContext.Request.Cookies["Login"]).FirstAsync();
            return View(await _repository.GetSelfMessages(userSpecification));
        }

        // GET: Messages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return Problem("Message was not found!");

            Message? msg = await _repository.GetMessageDetails(id);

            return (msg == null)
                   ? Problem("Message was not found!")
                   : Json(msg.ToJson());
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
                return Json("Message was added sucessfully!");
            }
            ViewData["UserId"] = new SelectList(_repository.GetUserByLogin(HttpContext.Request.Cookies["Login"]), "Id", "Login", message.UserId).First();
            return Problem("Add message error! Try again!");
        }

        // PUT: Messages/Edit/5
        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Mesage,UserId")] Message message)
        {

            if (ModelState.IsValid)
            {
                _repository.UpdateMessage(message);
                await _repository.SaveChanges();
                return Json("Message was updated sucessfully!");
            }
            return Problem("Message update error! Try again");
        }


        // DELETE: Messages/Delete/5
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if ((await _repository.GetAllMessages()).Count == 0)
                return Problem("The messages list is empty! Nothing to delete!");
            await _repository.DeleteMessage(id);
            await _repository.SaveChanges();
            return Json("Message was sucessfully deleted!");
        }
    }
}
