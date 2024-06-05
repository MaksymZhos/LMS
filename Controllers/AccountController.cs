using Microsoft.AspNetCore.Mvc;
using MyWebApp.Data;
using MyWebApp.Models;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace MyWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(AppDbContext context, ILogger<AccountController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Username: {Username}, Password: {Password}", model.Username, model.Password);

                var user = _context.Students
                    .FirstOrDefault(u => u.Name == model.Username && u.Password == model.Password);

                if (user != null)
                {
                    // Login successful, redirect to home page
                    return RedirectToAction("Index", "Home");
                }

                // Login failed
                ModelState.AddModelError("", "Invalid username or password");
            }

            return View(model);
        }
    }
}
