using Microsoft.AspNetCore.Mvc;
using AirlineTicketingSystemWebApp.Model;
using Microsoft.AspNetCore.Identity;
using AirlineTicketingSystemWebApp.Controllers;
using AirlineTicketingSystemWebApp.Models;
using AirlineTicketingSystemWebApp.Models.Dto;

public class AccountController : Controller
{
    private ILogger<AccountController> _logger;

    private readonly HttpClient _httpClient;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    private readonly string _ticketHistoryLink = "https://airlineticketingsystemapi.azurewebsites.net/api/v1/Ticket/GET/USER";

    public AccountController(ILogger<AccountController> logger,
                                HttpClient httpClient,
                                UserManager<User> userManager,
                                SignInManager<User> signInManager
                                )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _httpClient = httpClient;
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
    }

    // GET: Account/Register
    public ActionResult Register()
    {
        return View(new RegisterViewModel());
    }

    // POST: Account/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Register(RegisterViewModel model)
    {
        if ( (ModelState.IsValid) && (model.Password == model.ConfirmPassword) )
        {
            var user = new User()
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name,
                Surname = model.Surname,
                Birthdate = model.Birthdate,
                Nation = model.Nation,
                PassportNumber = model.PassportNumber,
                UserRole = "User",
                DateCreated = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }
            _logger.LogWarning(result.ToString());
        
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
                _logger.LogError(error.ToString());
            }
        }
        ModelState.AddModelError("", "Invalid Inputs.");
        _logger.LogWarning("Invalid Inputs");

        return View(model);
    }

    // GET: Account/Login
    public ActionResult Login()
    {
        return View(new LoginViewModel());
    }

    // POST: Account/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
    {
        if (ModelState.IsValid)
        {
            // Use SignInManager to sign in the user
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation("Login successful for user: {0}", model.Email);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                _logger.LogWarning("Invalid login attempt for user: {0}", model.Email);
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
        }
        else
        {
            _logger.LogWarning("Invalid input data");
        }

        model.Password = null; // Clear the password field
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Logout()
    {
        try
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }

    [HttpGet]
    public async Task<ActionResult> TicketHistory(TicketViewModel model)
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;
            var tickets = await GetTicketHistory(userId);
            model.Tickets = tickets;
            return View(model);
        }
        catch (Exception ex)
        {
            // Handle errors
            ViewBag.ErrorMessage = "An error occurred while retrieving tickets.";
            _logger.LogError(ex, "Error retrieving tickets");
            return View("TicketHistory", new TicketViewModel());
        }
    }

    private async Task<List<TicketDto>> GetTicketHistory(string userId)
    {
        var response = await _httpClient.GetAsync(_ticketHistoryLink + $"/{userId}");
        response.EnsureSuccessStatusCode(); // Ensure successful response
        var tickets = await response.Content.ReadAsAsync<List<TicketDto>>();

        return tickets;
    }

    private IActionResult RedirectToLocal(string returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
        else
        {
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}