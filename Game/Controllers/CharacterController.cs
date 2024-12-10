using Game.Data;
using Game.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Game.Controllers
{
    public class CharacterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CharacterController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Wyświetlanie listy postaci użytkownika
        public async Task<IActionResult> CharacterSelection()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "Home");
            }

            var user = await _context.Users.Include(u => u.Heroes).FirstOrDefaultAsync(u => u.Email == email);
            return View(user?.Heroes ?? new List<Hero>());
        }

        // Tworzenie nowej postaci
        [HttpGet]
        public IActionResult CreateHero()
        {
            return View(new Hero());
        }

        [HttpPost]
        public async Task<IActionResult> CreateHero(Hero hero)
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "Home");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (user.Heroes.Count >= 4)
            {
                TempData["ErrorMessage"] = "Możesz stworzyć maksymalnie 4 postacie.";
                return RedirectToAction("CharacterSelection");
            }

            hero.UserId = user.Id;
            hero.Level = 1;
            hero.Experience = 0;
            hero.Strength = 10;
            hero.Dexterity = 10;
            hero.Intelligence = 10;
            hero.Health = 100;
            hero.Money = 50;

            _context.Heroes.Add(hero);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Postać została stworzona!";
            return RedirectToAction("CharacterSelection");
        }

        // Edycja postaci
        [HttpGet]
        public async Task<IActionResult> EditHero(int id)
        {
            var hero = await _context.Heroes.FindAsync(id);
            if (hero == null)
            {
                TempData["ErrorMessage"] = "Nie znaleziono postaci.";
                return RedirectToAction("CharacterSelection");
            }

            return View(hero);
        }

        [HttpPost]
        public async Task<IActionResult> EditHero(int id, string name)
        {
            var hero = await _context.Heroes.FindAsync(id);
            if (hero == null)
            {
                TempData["ErrorMessage"] = "Nie znaleziono postaci.";
                return RedirectToAction("CharacterSelection");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                TempData["ErrorMessage"] = "Nazwa postaci nie może być pusta.";
                return View(hero);
            }

            hero.Name = name;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Postać została zaktualizowana!";
            return RedirectToAction("CharacterSelection");
        }

        // Usuwanie postaci
        [HttpPost]
        public async Task<IActionResult> DeleteHero(int id)
        {
            var hero = await _context.Heroes.Include(h => h.Items).FirstOrDefaultAsync(h => h.Id == id);
            if (hero == null)
            {
                TempData["ErrorMessage"] = "Nie znaleziono postaci.";
                return RedirectToAction("CharacterSelection");
            }

            _context.Heroes.Remove(hero);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Postać została usunięta.";
            return RedirectToAction("CharacterSelection");
        }
        public async Task<IActionResult> AddExperience(int heroId, int experience)
        {
            var hero = await _context.Heroes.FindAsync(heroId);
            if (hero == null)
            {
                TempData["ErrorMessage"] = "Nie znaleziono postaci.";
                return RedirectToAction("CharacterSelection", "Character");
            }

            hero.Experience += experience;

            // Sprawdź, czy bohater awansuje na wyższy poziom
            while (hero.Experience >= hero.Level * 100) // Zakładamy, że potrzeba 100 XP * poziom
            {
                hero.Experience -= hero.Level * 100;
                hero.Level++;
                hero.PointsToSpend += 5; // Przyznaj 5 punktów na poziom
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Zdobyto doświadczenie!";
            return RedirectToAction("HeroDetails", "Character", new { id = heroId });
        }
        [HttpPost]
        public async Task<IActionResult> EquipItem(int heroId, int itemId)
        {
            var hero = await _context.Heroes.Include(h => h.Items).FirstOrDefaultAsync(h => h.Id == heroId);
            var item = hero?.Items.FirstOrDefault(i => i.Id == itemId);

            if (hero == null || item == null)
            {
                TempData["ErrorMessage"] = "Nie znaleziono postaci lub przedmiotu.";
                return RedirectToAction("HeroDetails", new { id = heroId });
            }

            if (!item.IsEquipped)
            {
                var equippedItem = hero.Items.FirstOrDefault(i => i.Type == item.Type && i.IsEquipped);
                if (equippedItem != null)
                {
                    equippedItem.IsEquipped = false;
                    hero.Strength -= equippedItem.BonusStrength;
                    hero.Dexterity -= equippedItem.BonusDexterity;
                    hero.Intelligence -= equippedItem.BonusIntelligence;
                }

                item.IsEquipped = true;
                hero.Strength += item.BonusStrength;
                hero.Dexterity += item.BonusDexterity;
                hero.Intelligence += item.BonusIntelligence;
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Przedmiot został wyposażony.";
            return RedirectToAction("HeroDetails", new { id = heroId });
        }
        [HttpPost]
        public async Task<IActionResult> DistributePoints(int id, int strengthPoints, int dexterityPoints, int intelligencePoints)
        {
            var hero = await _context.Heroes.FindAsync(id);

            if (hero == null)
            {
                TempData["ErrorMessage"] = "Nie znaleziono postaci.";
                return RedirectToAction("CharacterSelection");
            }

            var totalPoints = strengthPoints + dexterityPoints + intelligencePoints;

            if (totalPoints > hero.PointsToSpend)
            {
                TempData["ErrorMessage"] = "Nie masz wystarczająco punktów do rozdania.";
                return RedirectToAction("HeroDetails", new { id });
            }

            hero.Strength += strengthPoints;
            hero.Dexterity += dexterityPoints;
            hero.Intelligence += intelligencePoints;
            hero.PointsToSpend -= totalPoints;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Punkty zostały rozdane!";
            return RedirectToAction("HeroDetails", new { id });
        }


    }
}
