using Game.Data;
using Game.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Game.Controllers
{
    public class BattleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BattleController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> StartBattle(int heroId, int enemyId)
        {
            var hero = await _context.Heroes.FindAsync(heroId);
            var enemy = await _context.Enemies.FindAsync(enemyId);

            if (hero == null || enemy == null)
            {
                TempData["ErrorMessage"] = "Nie znaleziono bohatera lub wroga.";
                return RedirectToAction("CharacterSelection", "Character");
            }

            return View(new BattleViewModel
            {
                Hero = hero,
                Enemy = enemy
            });
        }

        [HttpPost]
        public async Task<IActionResult> Attack(int heroId, int enemyId)
        {
            var hero = await _context.Heroes.FindAsync(heroId);
            var enemy = await _context.Enemies.FindAsync(enemyId);

            if (hero == null || enemy == null)
            {
                TempData["ErrorMessage"] = "Nie znaleziono bohatera lub wroga.";
                return RedirectToAction("CharacterSelection", "Character");
            }

            // Logika walki
            var heroDamage = hero.Strength - enemy.Defense;
            if (heroDamage > 0) enemy.Health -= heroDamage;

            if (enemy.Health <= 0)
            {
                // Bohater wygrywa
                hero.Experience += enemy.RewardExperience;
                hero.Money += enemy.RewardGold;

                // Sprawdź awans na wyższy poziom
                while (hero.Experience >= hero.Level * 100)
                {
                    hero.Experience -= hero.Level * 100;
                    hero.Level++;
                    hero.PointsToSpend += 5;
                }

                _context.Enemies.Remove(enemy);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Pokonałeś {enemy.Name}! Zdobyłeś {enemy.RewardExperience} XP i {enemy.RewardGold} złota.";
                return RedirectToAction("HeroDetails", "Character", new { id = heroId });
            }

            // Wróg kontratakuje
            var enemyDamage = enemy.Strength - hero.Defense;
            if (enemyDamage > 0) hero.Health -= enemyDamage;

            if (hero.Health <= 0)
            {
                TempData["ErrorMessage"] = "Zostałeś pokonany! Odzyskaj siły przed kolejną walką.";
                return RedirectToAction("CharacterSelection", "Character");
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("StartBattle", new { heroId, enemyId });
        }
    }
}
