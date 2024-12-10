using Game.Data;
using Game.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Game.Controllers
{
    public class DailyQuestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DailyQuestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var dailyQuests = await _context.DailyQuests.ToListAsync();
            return View(dailyQuests);
        }

        [HttpGet]
        public async Task<IActionResult> DailyQuest(int heroId)
        {
            var hero = await _context.Heroes.FindAsync(heroId);
            var dailyQuest = await _context.DailyQuests.FirstOrDefaultAsync();

            if (dailyQuest == null || hero == null)
            {
                TempData["ErrorMessage"] = "Nie znaleziono bohatera lub zadania dziennego.";
                return RedirectToAction("HeroDetails", "Character", new { id = heroId });
            }

            if (DateTime.Now < dailyQuest.ResetTime)
            {
                TempData["ErrorMessage"] = "Zadanie dzienne już zostało wykonane.";
                return RedirectToAction("HeroDetails", "Character", new { id = heroId });
            }

            hero.Experience += dailyQuest.RewardExperience;
            hero.Money += dailyQuest.RewardGold;

            dailyQuest.ResetTime = DateTime.Now.AddDays(1);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Zadanie ukończone! Zdobyłeś {dailyQuest.RewardExperience} doświadczenia i {dailyQuest.RewardGold} złota.";
            return RedirectToAction("HeroDetails", "Character", new { id = heroId });
        }
    }
}
