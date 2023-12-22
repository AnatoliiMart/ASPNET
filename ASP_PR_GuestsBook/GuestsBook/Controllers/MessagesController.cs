using GuestsBook.Models;
using GuestsBook.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GuestsBook.Controllers
{
    public class MessagesController : Controller
    {
        private readonly IMessagesRepository _repository;
        public MessagesController(IMessagesRepository repository) => _repository = repository;

        // GET: Messages
        public async Task<IActionResult> Index() => View(await _repository.GetAllMessages());

        public async Task<IActionResult> MyMessages()
        {
            User userSpecification = await _repository.GetUserByLogin(HttpContext.Session.GetString("Login")).FirstAsync();
            return View(await _repository.GetSelfMessages(userSpecification));
        }

        // GET: Messages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            Message? msg = await _repository.GetMessageDetails(id);

            if (msg == null)
                return NotFound();

            return View(msg);
        }

        // GET: Messages/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_repository.GetUserByLogin(HttpContext.Session.GetString("Login")), "Id", "Login");
            return View();
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
                return RedirectToAction(nameof(MyMessages));
            }
            ViewData["UserId"] = new SelectList(_repository.GetUserByLogin(HttpContext.Session.GetString("Login")), "Id", "Login", message.UserId);
            return View(message);
        }

        // GET: Messages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var message = await _repository.GetMessage(id);

            if (message == null)
                return NotFound();

            ViewData["UserId"] = new SelectList(_repository.GetUserByLogin(HttpContext.Session.GetString("Login")), "Id", "Login", message.UserId);

            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Mesage,UserId")] Message message)
        {
            if (id != message.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _repository.UpdateMessage(message);
                    await _repository.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _repository.MessageExists(message.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(MyMessages));
            }
            ViewData["UserId"] = new SelectList(_repository.GetUserByLogin(HttpContext.Session.GetString("Login")), "Id", "Login", message.UserId);
            return View(message);
        }

        // GET: Messages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var message = await _repository.GetMessageDetails(id);

            if (message == null)
                return NotFound();

            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repository.DeleteMessage(id);
            await _repository.SaveChanges();
            return RedirectToAction(nameof(MyMessages));
        }


    }
}
