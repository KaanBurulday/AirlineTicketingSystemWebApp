using AirlineTicketingSystemWebApp.Model;
using AirlineTicketingSystemWebApp.Models;
using AirlineTicketingSystemWebApp.Models.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AirlineTicketingSystemWebApp.Controllers
{
    public class TicketController : Controller
    {
        private ILogger<FlightController> _logger;
        private readonly HttpClient _httpClient;
        private readonly UserManager<User> _userManager;
        private readonly string _getFlightLink = "https://airlineticketingsystemapi.azurewebsites.net/api/v1/Flight/GET/CODE";
        private readonly string _buyTicketLink = "https://airlineticketingsystemapi.azurewebsites.net/api/v1/Booking/BUY";

        public TicketController(ILogger<FlightController> logger,
                                UserManager<User> userManager,
                                HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<IActionResult> BuyTicket(string Code)
        {
            var response = await _httpClient.GetAsync(_getFlightLink + $"/{Code}");
            response.EnsureSuccessStatusCode();
            var flightDto = await response.Content.ReadAsAsync<FlightSearchResultDto>();

            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            BuyTicketViewModel model = new BuyTicketViewModel()
            {
                FlightCode = flightDto.Code,
                OccupiedSeatNumbers = flightDto.OccupiedSeats.ToList(),
                UserId = userId
            };

            for (int i = 0; i < flightDto.OccupiedSeats.Count + flightDto.AvailableSeatCount; i++)
                model.AvailableSeatNumbers.Add(i);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> BuyTicket(BuyTicketViewModel model)
        {
            TempData["SuccessMessage"] = "Ticket bought successfully!";
            return RedirectToAction(nameof(AccountController.TicketHistory), "Account");
            //if (ModelState.IsValid)
            //{
            //    var ticketDto = new TicketDto()
            //    {
            //        Code = "",
            //        Price = 150,
            //        DateCreated = DateTime.Now,
            //        FlightCode = model.FlightCode,
            //        BuyWithMilesPoints = model.BuyWithMilesPoints,
            //        SeatNumbers = model.SeatNumbers,
            //        UserId = model.UserId
            //    };

            //    var response = await _httpClient.PostAsJsonAsync(_buyTicketLink, ticketDto);
            //    if (response.IsSuccessStatusCode)
            //    {
            //        // If successful, you can redirect to another action or display a success message
            //        TempData["SuccessMessage"] = "Flight added successfully!";
            //        _logger.LogInformation($"Ticket bought successfully for the flight: {model.FlightCode}");
            //        return RedirectToAction(nameof(AccountController.TicketHistory), "TicketHistory");
            //    }
            //    else
            //    {
            //        // Handle errors (you can log the error, add model errors, etc.)
            //        var errorContent = await response.Content.ReadAsStringAsync();
            //        ModelState.AddModelError("", $"Error adding flight: {errorContent}");
            //        _logger.LogError($"Error adding flight: {errorContent}");
            //    }
            //}
            //ModelState.AddModelError("", "Invalid Inputs.");
            //_logger.LogWarning("Invalid Inputs");

            //return View(model);
        }
    }
}
