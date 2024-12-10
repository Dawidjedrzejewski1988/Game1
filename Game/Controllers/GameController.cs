using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Game.Data;
using Game.Models;

namespace Game.Controllers
{
    public class GameController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GameController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("CharacterSelection", "Character");
            }
            return View();
        }

        public async Task<IActionResult> Missions(int heroId)
        {
            var hero = await _context.Heroes
                .Include(h => h.HeroQuests)
                .ThenInclude(hq => hq.Quest)
                .FirstOrDefaultAsync(h => h.Id == heroId);

            if (hero == null)
            {
                TempData["ErrorMessage"] = "Nie znaleziono bohatera.";
                return RedirectToAction("CharacterSelection", "Character");
            }

            var availableQuests = await _context.Quests
                .Where(q => !hero.HeroQuests.Any(hq => hq.QuestId == q.Id))
                .ToListAsync();

            return View(new MissionsViewModel
            {
                Hero = hero,
                AvailableQuests = availableQuests,
                AssignedQuests = hero.HeroQuests.Where(hq => !hq.IsCompleted).Select(hq => hq.Quest).ToList()
            });
        }


        [HttpPost]
        public async Task<IActionResult> AssignQuest(int heroId, int questId)
        {
            var hero = await _context.Heroes.FindAsync(heroId);
            var quest = await _context.Quests.FindAsync(questId);

            if (hero == null || quest == null)
            {
                TempData["ErrorMessage"] = "Nie znaleziono postaci lub questa.";
                return RedirectToAction("Missions", new { heroId });
            }

            if (_context.HeroQuests.Any(hq => hq.HeroId == heroId && hq.QuestId == questId))
            {
                TempData["ErrorMessage"] = "Quest jest już przypisany do tego bohatera.";
                return RedirectToAction("Missions", new { heroId });
            }

            var heroQuest = new HeroQuest
            {
                HeroId = heroId,
                QuestId = questId,
                IsCompleted = false,
                StartTime = DateTime.Now
            };

            _context.HeroQuests.Add(heroQuest);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Quest został przypisany!";
            return RedirectToAction("Missions", new { heroId });
        }


        public async Task<IActionResult> Shop(int heroId)
        {
            var hero = await _context.Heroes
                .Include(h => h.Items)
                .FirstOrDefaultAsync(h => h.Id == heroId);

            if (hero == null)
            {
                return NotFound();
            }

            var items = await _context.Items.Where(i => i.IsForSale).ToListAsync();

            if (!items.Any())
            {
                TempData["ErrorMessage"] = "Brak przedmiotów dostępnych do zakupu.";
            }

            return View(new ShopViewModel
            {
                Hero = hero,
                Items = items
            });
        }

        [HttpPost]
        public async Task<IActionResult> PurchaseItem(int heroId, int itemId)
        {
            var hero = await _context.Heroes.Include(h => h.Items).FirstOrDefaultAsync(h => h.Id == heroId);
            var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == itemId);

            if (hero == null || item == null)
            {
                TempData["ErrorMessage"] = "Nie znaleziono postaci lub przedmiotu.";
                return RedirectToAction("Shop", new { heroId });
            }

            if (hero.Money < item.Price)
            {
                TempData["ErrorMessage"] = "Nie masz wystarczająco złota.";
                return RedirectToAction("Shop", new { heroId });
            }

            hero.Money -= item.Price;
            hero.Items.Add(item);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Zakupiono przedmiot!";
            return RedirectToAction("Shop", new { heroId });
        }

        [HttpPost]
        public async Task<IActionResult> SellItem(int heroId, int itemId)
        {
            var hero = await _context.Heroes.Include(h => h.Items).FirstOrDefaultAsync(h => h.Id == heroId);
            var item = hero?.Items.FirstOrDefault(i => i.Id == itemId);

            if (hero == null || item == null)
            {
                TempData["ErrorMessage"] = "Nie znaleziono postaci lub przedmiotu.";
                return RedirectToAction("Shop", new { heroId });
            }

            hero.Money += item.Price / 2; // Cena sprzedaży to połowa ceny zakupu
            hero.Items.Remove(item);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Przedmiot został sprzedany!";
            return RedirectToAction("Shop", new { heroId });
        }

        public async Task<IActionResult> CharacterSelection()
        {
            var email = HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "Home");
            }

            var user = await _context.Users.Include(u => u.Heroes).FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || user.Heroes == null || !user.Heroes.Any())
            {
                TempData["ErrorMessage"] = "Nie znaleziono bohaterów dla tego użytkownika.";
            }

            return View(user?.Heroes ?? new List<Hero>());
        }
        [HttpPost]
        public async Task<IActionResult> CompleteQuest(int heroId, int questId)
        {
            var heroQuest = await _context.HeroQuests
                .Include(hq => hq.Hero)
                .Include(hq => hq.Quest)
                .FirstOrDefaultAsync(hq => hq.HeroId == heroId && hq.QuestId == questId);

            if (heroQuest == null)
            {
                TempData["ErrorMessage"] = "Nie znaleziono przypisanej misji.";
                return RedirectToAction("Missions", new { heroId });
            }

            if (heroQuest.IsCompleted)
            {
                TempData["ErrorMessage"] = "Ta misja została już ukończona.";
                return RedirectToAction("Missions", new { heroId });
            }

            heroQuest.IsCompleted = true;
            heroQuest.Hero.Experience += heroQuest.Quest.RewardExperience;

            // Sprawdź awans na wyższy poziom
            while (heroQuest.Hero.Experience >= heroQuest.Hero.Level * 100)
            {
                heroQuest.Hero.Experience -= heroQuest.Hero.Level * 100;
                heroQuest.Hero.Level++;
                heroQuest.Hero.PointsToSpend += 5;
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Misja ukończona! Zdobyłeś {heroQuest.Quest.RewardExperience} doświadczenia.";
            return RedirectToAction("Missions", new { heroId });
        }
        [HttpPost]
        public async Task<IActionResult> UpgradeItem(int heroId, int itemId)
        {
            var hero = await _context.Heroes.Include(h => h.Items).FirstOrDefaultAsync(h => h.Id == heroId);
            var item = hero?.Items.FirstOrDefault(i => i.Id == itemId);

            if (hero == null || item == null)
            {
                TempData["ErrorMessage"] = "Nie znaleziono postaci lub przedmiotu.";
                return RedirectToAction("HeroDetails", new { id = heroId });
            }

            var upgradeCost = (item.UpgradeLevel + 1) * 100; // Koszt rośnie z poziomem ulepszenia

            if (hero.Money < upgradeCost)
            {
                TempData["ErrorMessage"] = "Nie masz wystarczająco złota.";
                return RedirectToAction("HeroDetails", new { id = heroId });
            }

            hero.Money -= upgradeCost;
            item.UpgradeLevel++;
            item.BonusStrength += 2;
            item.BonusDexterity += 2;
            item.BonusIntelligence += 2;
            item.BonusDefense += 1;
            item.BonusHealth += 5;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Przedmiot został ulepszony!";
            return RedirectToAction("HeroDetails", new { id = heroId });
        }
        public async Task<IActionResult> Leaderboard()
        {
            var heroes = await _context.Heroes
                .OrderByDescending(h => h.Level)
                .ThenByDescending(h => h.Experience)
                .ToListAsync();

            return View(heroes);
        }
    }
}
