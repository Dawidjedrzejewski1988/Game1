using Game.Data;
using Game.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Game.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult AdminDashboard()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return Unauthorized();
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> QuestList()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return Unauthorized();
            }

            var quests = await _context.Quests.ToListAsync();
            return View(quests);
        }

        [HttpGet]
        public async Task<IActionResult> ItemList()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return Unauthorized();
            }

            var items = await _context.Items.ToListAsync();
            return View(items);
        }

        [HttpGet]
        public IActionResult AddQuest()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return Unauthorized();
            }

            var difficulties = new List<string> { "Łatwy", "Średni", "Trudny" };
            ViewBag.Difficulties = difficulties;

            return View(new Quest());
        }

        [HttpPost]
        public async Task<IActionResult> AddQuest(Quest quest)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                _context.Quests.Add(quest);
                await _context.SaveChangesAsync();
                return RedirectToAction("AdminDashboard");
            }

            return View(quest);
        }

        [HttpGet]
        public IActionResult AddItem()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return Unauthorized();
            }

            return View(new Item());
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(Item item)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                _context.Items.Add(item);
                await _context.SaveChangesAsync();

                return RedirectToAction("ItemList");
            }

            return View(item);
        }
        [HttpGet]
        public async Task<IActionResult> EditItem(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return Unauthorized();
            }

            var item = await _context.Items.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> EditItem(Item item)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Items.Update(item);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ItemList");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Items.Any(e => e.Id == item.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(item);
        }

        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction("ItemList");
        }

        [HttpGet]
        public async Task<IActionResult> EditQuest(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return Unauthorized();
            }

            var difficulties = new List<string> { "Łatwy", "Średni", "Trudny" };
            ViewBag.Difficulties = difficulties;

            var quest = await _context.Quests.FindAsync(id);
            if (quest == null)
            {
                return NotFound();
            }

            return View(quest);
        }

        [HttpPost]
        public async Task<IActionResult> EditQuest(Quest quest)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                _context.Quests.Update(quest);
                await _context.SaveChangesAsync();
                return RedirectToAction("QuestList");
            }

            return View(quest);
        }

        public async Task<IActionResult> DeleteQuest(int id)
        {
            var quest = await _context.Quests.FindAsync(id);
            if (quest == null)
            {
                return NotFound();
            }

            _context.Quests.Remove(quest);
            await _context.SaveChangesAsync();
            return RedirectToAction("QuestList");
        }
    }
}
