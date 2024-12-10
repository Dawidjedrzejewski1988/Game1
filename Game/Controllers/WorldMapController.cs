using Game.Data;
using Game.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Game.Controllers
{
    public class WorldMapController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WorldMapController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var locations = await _context.Locations.Include(l => l.Quests).ToListAsync();
            return View(locations);
        }

        [HttpGet]
        public async Task<IActionResult> LocationDetails(int id)
        {
            var location = await _context.Locations.Include(l => l.Quests).FirstOrDefaultAsync(l => l.Id == id);
            if (location == null)
            {
                TempData["ErrorMessage"] = "Nie znaleziono tej lokacji.";
                return RedirectToAction("Index");
            }

            return View(location);
        }

        [HttpPost]
        public async Task<IActionResult> Travel(int heroId, int locationId)
        {
            var hero = await _context.Heroes.FindAsync(heroId);
            var location = await _context.Locations.FindAsync(locationId);

            if (hero == null || location == null || !location.IsAccessible)
            {
                TempData["ErrorMessage"] = "Nie możesz podróżować do tej lokacji.";
                return RedirectToAction("Index");
            }

            hero.LocationId = locationId; // Zakładam, że Hero ma pole `LocationId`
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Podróżowałeś do {location.Name}!";
            return RedirectToAction("Index");
        }
    }
}
