using Game.Data;
using Game.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Game.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context; 
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Hero");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("Email", "U¿ytkownik o podanym adresie e-mail nie istnieje.");
                    return View(model);
                }

                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(model.Password, user.HashedPassword);
                if (!isPasswordValid)
                {
                    ModelState.AddModelError("Password", "Nieprawid³owe has³o.");
                    return View(model);
                }

                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("UserRole", user.Role);

                if (user.Role == "Admin")
                {
                    return RedirectToAction("AdminDashboard", "Admin");
                }

                return RedirectToAction("CharacterSelection", "Character");
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userExists = await _context.Users.AnyAsync(u => u.Email == model.Email);
                if (userExists)
                {
                    ModelState.AddModelError("Email", "U¿ytkownik o podanym adresie e-mail ju¿ istnieje.");
                    return View(model);
                }

                var user = new User
                {
                    Email = model.Email,
                    HashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password),
                    Role = "User"
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Rejestracja przebieg³a pomyœlnie! Mo¿esz teraz siê zalogowaæ.";
             
                return RedirectToAction("Login");
            }

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
