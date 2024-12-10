using Game.Data;
using Game.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Game.Controllers
{
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MessagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Inbox(int userId)
        {
            var messages = await _context.Messages
                .Include(m => m.Sender)
                .Where(m => m.ReceiverId == userId)
                .OrderByDescending(m => m.SentAt)
                .ToListAsync();

            return View(messages);
        }

        [HttpGet]
        public async Task<IActionResult> Sent(int userId)
        {
            var messages = await _context.Messages
                .Include(m => m.Receiver)
                .Where(m => m.SenderId == userId)
                .OrderByDescending(m => m.SentAt)
                .ToListAsync();

            return View(messages);
        }

        [HttpGet]
        public IActionResult Compose(int senderId)
        {
            var users = _context.Users.ToList();
            ViewBag.Users = users;

            return View(new Message { SenderId = senderId });
        }

        [HttpPost]
        public async Task<IActionResult> Compose(Message message)
        {
            if (ModelState.IsValid)
            {
                _context.Messages.Add(message);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Wiadomość została wysłana!";
                return RedirectToAction("Sent", new { userId = message.SenderId });
            }

            return View(message);
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int messageId, int userId)
        {
            var message = await _context.Messages.FindAsync(messageId);
            if (message == null || message.ReceiverId != userId)
            {
                return NotFound();
            }

            message.IsRead = true;
            await _context.SaveChangesAsync();

            return RedirectToAction("Inbox", new { userId });
        }
    }
}
