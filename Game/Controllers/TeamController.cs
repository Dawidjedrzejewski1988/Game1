using Game.Data;
using Game.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Game.Controllers
{
    public class TeamController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TeamController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var teams = await _context.Teams.Include(t => t.Members).ToListAsync();
            return View(teams);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeam(string name, int heroId)
        {
            var hero = await _context.Heroes.FindAsync(heroId);

            if (hero == null)
            {
                TempData["ErrorMessage"] = "Nie znaleziono bohatera.";
                return RedirectToAction("Index");
            }

            var team = new Team { Name = name };
            team.Members.Add(hero);

            _context.Teams.Add(team);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Drużyna {name} została stworzona!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> JoinTeam(int teamId, int heroId)
        {
            var team = await _context.Teams.Include(t => t.Members).FirstOrDefaultAsync(t => t.Id == teamId);
            var hero = await _context.Heroes.FindAsync(heroId);

            if (team == null || hero == null)
            {
                TempData["ErrorMessage"] = "Nie znaleziono drużyny lub bohatera.";
                return RedirectToAction("Index");
            }

            if (team.Members.Contains(hero))
            {
                TempData["ErrorMessage"] = "Już jesteś w tej drużynie.";
                return RedirectToAction("Index");
            }

            team.Members.Add(hero);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Dołączyłeś do drużyny {team.Name}!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> TeamQuests(int teamId)
        {
            var team = await _context.Teams.Include(t => t.TeamQuests).ThenInclude(tq => tq.Quest).FirstOrDefaultAsync(t => t.Id == teamId);

            if (team == null)
            {
                TempData["ErrorMessage"] = "Nie znaleziono drużyny.";
                return RedirectToAction("Index");
            }

            return View(team);
        }

        [HttpPost]
        public async Task<IActionResult> StartTeamQuest(int teamId, int questId)
        {
            var team = await _context.Teams.FindAsync(teamId);
            var quest = await _context.Quests.FindAsync(questId);

            if (team == null || quest == null)
            {
                TempData["ErrorMessage"] = "Nie znaleziono drużyny lub questa.";
                return RedirectToAction("TeamQuests", new { teamId });
            }

            var teamQuest = new TeamQuest
            {
                TeamId = teamId,
                QuestId = questId,
                StartTime = DateTime.Now,
                IsCompleted = false
            };

            _context.TeamQuests.Add(teamQuest);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Misja drużynowa rozpoczęta!";
            return RedirectToAction("TeamQuests", new { teamId });
        }
    }
}
