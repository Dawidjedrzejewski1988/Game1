using Game.Data;
using Game.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Game.Controllers
{
    public class CraftingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CraftingController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var recipes = await _context.CraftingRecipes
                .Include(r => r.ResultItem)
                .Include(r => r.Ingredients)
                    .ThenInclude(i => i.Item)
                .ToListAsync();

            return View(recipes);
        }

        [HttpPost]
        public async Task<IActionResult> CraftItem(int recipeId, int heroId)
        {
            var recipe = await _context.CraftingRecipes
                .Include(r => r.Ingredients)
                .FirstOrDefaultAsync(r => r.Id == recipeId);

            var hero = await _context.Heroes
                .Include(h => h.Items)
                .FirstOrDefaultAsync(h => h.Id == heroId);

            if (recipe == null || hero == null)
            {
                TempData["ErrorMessage"] = "Nie znaleziono przepisu lub bohatera.";
                return RedirectToAction("Index");
            }

            // Sprawdź, czy gracz ma wszystkie wymagane składniki
            foreach (var ingredient in recipe.Ingredients)
            {
                var heroItem = hero.Items.FirstOrDefault(i => i.Id == ingredient.ItemId);
                if (heroItem == null || heroItem.Quantity < ingredient.Quantity)
                {
                    TempData["ErrorMessage"] = "Nie masz wystarczających składników.";
                    return RedirectToAction("Index");
                }
            }

            // Odejmij składniki
            foreach (var ingredient in recipe.Ingredients)
            {
                var heroItem = hero.Items.FirstOrDefault(i => i.Id == ingredient.ItemId);
                heroItem.Quantity -= ingredient.Quantity;
            }

            // Dodaj wytworzony przedmiot
            var craftedItem = await _context.Items.FindAsync(recipe.ResultItemId);
            if (craftedItem != null)
            {
                hero.Items.Add(craftedItem);
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Przedmiot został wytworzony!";
            return RedirectToAction("Index");
        }
    }
}
