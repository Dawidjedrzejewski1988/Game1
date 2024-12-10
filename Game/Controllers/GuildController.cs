using Game.Data;
using Game.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Game.Controllers
{
    public class GuildController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GuildController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var guilds = await _context.Guilds.Include(g => g.Members).ToListAsync();
            return View(guilds);
        }

        [HttpPost]
        public async Task<IActionResult> JoinGuild(int heroId, int guildId)
        {
            var hero = await _context.Heroes.FindAsync(heroId);
            var guild = await _context.Guilds.Include(g => g.Members).FirstOrDefaultAsync(g => g.Id == guildId);

            if (hero == null || guild == null)
            {
                TempData["ErrorMessage"] = "Nie znaleziono bohatera lub gildii.";
                return RedirectToAction("Index");
            }

            if (guild.Members.Contains(hero))
            {
                TempData["ErrorMessage"] = "Już jesteś członkiem tej gildii.";
                return RedirectToAction("Index");
            }

            guild.Members.Add(hero);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Dołączyłeś do gildii {guild.Name}!";
            return RedirectToAction("Index");
        }
    }
}
