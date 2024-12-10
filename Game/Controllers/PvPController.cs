using Game.Data;
using Game.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Game.Controllers
{
    public class PvPController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PvPController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Challenge(int heroId)
        {
            var hero = await _context.Heroes.FindAsync(heroId);
            if (hero == null)
            {
                TempData["ErrorMessage"] = "Nie znaleziono bohatera.";
                return RedirectToAction("CharacterSelection", "Character");
            }

            var opponents = await _context.Heroes
                .Where(h => h.Id != heroId)
                .ToListAsync();

            return View(new PvPChallengeViewModel
            {
                Hero = hero,
                Opponents = opponents
            });
        }

        [HttpPost]
        public async Task<IActionResult> StartPvP(int heroId, int opponentId)
        {
            var hero = await _context.Heroes.FindAsync(heroId);
            var opponent = await _context.Heroes.FindAsync(opponentId);

            if (hero == null || opponent == null)
            {
                TempData["ErrorMessage"] = "Nie znaleziono bohatera lub przeciwnika.";
                return RedirectToAction("Challenge", new { heroId });
            }

            return View(new PvPBattleViewModel
            {
                Hero = hero,
                Opponent = opponent
            });
        }

        [HttpPost]
        public async Task<IActionResult> PvPAttack(int heroId, int opponentId)
        {
            var hero = await _context.Heroes.FindAsync(heroId);
            var opponent = await _context.Heroes.FindAsync(opponentId);

            if (hero == null || opponent == null)
            {
                TempData["ErrorMessage"] = "Nie znaleziono bohatera lub przeciwnika.";
                return RedirectToAction("Challenge", new { heroId });
            }

            // Bohater atakuje przeciwnika
            var heroDamage = hero.Strength - opponent.Defense;
            if (heroDamage > 0) opponent.Health -= heroDamage;

            if (opponent.Health <= 0)
            {
                TempData["SuccessMessage"] = $"{hero.Name} pokonał {opponent.Name}!";
                hero.Money += 100; // Nagroda
                await _context.SaveChangesAsync();
                return RedirectToAction("CharacterSelection", "Character");
            }

            // Przeciwnik kontratakuje
            var opponentDamage = opponent.Strength - hero.Defense;
            if (opponentDamage > 0) hero.Health -= opponentDamage;

            if (hero.Health <= 0)
            {
                TempData["ErrorMessage"] = $"{opponent.Name} pokonał {hero.Name}.";
                await _context.SaveChangesAsync();
                return RedirectToAction("CharacterSelection", "Character");
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("StartPvP", new { heroId, opponentId });
        }
    }
}
