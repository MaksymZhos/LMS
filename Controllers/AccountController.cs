using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using MyWebApp.Models;
using Microsoft.Extensions.Logging;

public class AccountController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, ILogger<AccountController> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
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
            _logger.LogInformation("Attempting to log in user: {Username}", model.Username);

            // Log all users in the database
            var users = _userManager.Users.ToList();
            _logger.LogInformation("Current users in the database: {Users}", users.Select(u => new { u.UserName, u.NormalizedUserName }));

            // Normalize username and log it
            var normalizedUserName = _userManager.NormalizeName(model.Username);
            _logger.LogInformation("Normalized username: {NormalizedUserName}", normalizedUserName);

            var user = await _userManager.FindByNameAsync(normalizedUserName);
            if (user == null)
            {
                _logger.LogWarning("Login attempt failed: User not found.");
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation("User {Username} logged in successfully.", model.Username);
                return RedirectToAction("Index", "Home");
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User {Username} account locked out.", model.Username);
                return View("Lockout");
            }
            else
            {
                _logger.LogWarning("Login attempt failed for user {Username}.", model.Username);
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }
        }
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User logged out.");
        return RedirectToAction("Login", "Account");
    }
}
