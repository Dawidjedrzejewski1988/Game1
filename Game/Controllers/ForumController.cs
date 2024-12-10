using Game.Data;
using Game.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace Game.Controllers
{
    public class ForumController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ForumController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var threads = _context.ForumThreads
                .Include(t => t.User)
                .Include(t => t.Replies)
                .ToList();
            return View(threads);
        }

        public IActionResult Show(int id)
        {
            var thread = _context.ForumThreads
                .Include(t => t.User)
                .FirstOrDefault(t => t.Id == id);

            if (thread == null)
            {
                return NotFound();
            }

            var replies = _context.Replies
                .Where(r => r.ThreadId == id)
                .Include(r => r.User)
                .ToList();

            var viewModel = new ThreadRepliesViewModel
            {
                Thread = thread,
                Replies = replies
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult CreateThread()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateThread(ForumThread forumThread)
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "Home");
            }

            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == email);
                if (user != null)
                {
                    forumThread.UserId = user.Id;
                    forumThread.User = user;
                    forumThread.CreatedAt = DateTime.Now;

                    _context.ForumThreads.Add(forumThread);
                    _context.SaveChanges();

                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Nie udało się znaleźć użytkownika.");
            }
            return View(forumThread);
        }

       

        [HttpGet]
        public IActionResult EditThread(int id)
        {
            var thread = _context.ForumThreads.Include(t => t.User).FirstOrDefault(t => t.Id == id);

            if (thread == null)
            {
                return NotFound();
            }

            var userId = HttpContext.Session.GetInt32("UserId");

            if (thread.UserId != userId)
            {
                return Unauthorized();
            }

            return View(thread);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditThread(int id, ForumThread forumThread)
        {
            if (ModelState.IsValid)
            {
                var thread = _context.ForumThreads.FirstOrDefault(t => t.Id == id);
                if (thread == null)
                {
                    return NotFound();
                }

                var userId = HttpContext.Session.GetInt32("UserId");

                if (thread.UserId != userId)
                {
                    return Unauthorized();
                }

                thread.Title = forumThread.Title;
                thread.Content = forumThread.Content;
                _context.SaveChanges();

                return RedirectToAction("Index", "Forum");
            }

            return View(forumThread);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteThread(int id)
        {
            var thread = _context.ForumThreads.FirstOrDefault(t => t.Id == id);
            if (thread == null)
            {
                return NotFound();
            }

            var email = HttpContext.Session.GetString("UserEmail");
            if (HttpContext.Session.GetString("UserRole") != "Admin") 
            {
                return Unauthorized(); 
            }

            _context.ForumThreads.Remove(thread);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReply(int threadId, string content)
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
            {
                TempData["ErrorMessage"] = "Musisz być zalogowany, aby odpowiedzieć.";
                return RedirectToAction("Login", "Home");
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                TempData["ErrorMessage"] = "Treść odpowiedzi nie może być pusta.";
                return RedirectToAction("Show", new { id = threadId });
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Nie znaleziono użytkownika.";
                return RedirectToAction("Show", new { id = threadId });
            }

            var reply = new Reply
            {
                ThreadId = threadId,
                Content = content,
                UserId = user.Id,
                CreatedAt = DateTime.Now
            };

            _context.Replies.Add(reply);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Odpowiedź została dodana!";
            return RedirectToAction("Show", new { id = threadId });
        }

    }
}