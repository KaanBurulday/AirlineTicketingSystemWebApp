using AirlineTicketingSystemWebApp.Models;
using AirlineTicketingSystemWebApp.Models.Dto;
using AirlineTicketingSystemWebApp.Source.Interfaces;
using AirlineTicketingSystemWebApp.Source.Svc.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;

namespace AirlineTicketingSystemWebApp.Controllers
{
    public class FlightController : Controller
    {
        private IJwtTokenService _jwtTokenService;
        private IUserService _userService;
        private ILogger<FlightController> _logger;
        private readonly HttpClient _httpClient;
        private readonly string _insertFlightLink = "http://airlineticketingsystemapi.azurewebsites.net/api/v1/Flight/POST";
        private readonly string _getAirportsLink = "http://airlineticketingsystemapi.azurewebsites.net/api/v1/Airport/GET";
        private readonly string _getPlanesLink = "http://airlineticketingsystemapi.azurewebsites.net/api/v1/Plane/GET";
        private readonly string _getFilteredFlightsLink = "http://airlineticketingsystemapi.azurewebsites.net/api/v1/Flight/POST/Filtered";
        private readonly string _getFlightsLink = "http://airlineticketingsystemapi.azurewebsites.net/api/v1/Flight/GET";

        public FlightController(ILogger<FlightController> logger, 
                                HttpClient httpClient, 
                                IJwtTokenService jwtTokenService,
                                IUserService userService)
        {
            _logger = logger;
            _httpClient = httpClient;
            _jwtTokenService = jwtTokenService;
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddFlight()
        {
            return View(new AddFlightViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Admin")]
        public async Task<ActionResult> AddFlight(AddFlightViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.GetUserAsync(); // Implement this method to get the current user

                // Generate token for the current user
                var token = _jwtTokenService.GenerateToken(user);

                var flightDto = new FlightDto()
                {
                    Code = model.Code,
                    DepartureDate = model.DepartureDate,
                    DepartureAirportCode = model.DepartureAirportCode,
                    ArrivalAirportCode = model.ArrivalAirportCode,
                    PlaneCode = model.PlaneCode,
                };

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                
                _logger.LogInformation("User {User} is submitting add flight form", User.Identity.Name);
                var response = await _httpClient.PostAsJsonAsync(_insertFlightLink, flightDto);
                if (response.IsSuccessStatusCode)
                {
                    // If successful, you can redirect to another action or display a success message
                    TempData["SuccessMessage"] = "Flight added successfully!";
                    _logger.LogInformation($"Flight added successfully: {model.Code}");
                    return RedirectToAction(nameof(FlightController.Flights), "Flight");
                }
                else
                {
                    // Handle errors (you can log the error, add model errors, etc.)
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError("", $"Error adding flight: {errorContent}");
                    _logger.LogError($"Error adding flight: {errorContent}");
                }
            }
            ModelState.AddModelError("", "Invalid Inputs.");
            _logger.LogWarning("Invalid Inputs");

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Flights(FlightFilterViewModel model)
        {
            try
            {
                var filteredFlights = await GetFilteredFlights(model);
                model.flightDtos = filteredFlights;
                return View(model);
            }
            catch (Exception ex)
            {
                // Handle errors
                ViewBag.ErrorMessage = "An error occurred while retrieving flights.";
                _logger.LogError(ex, "Error retrieving flights");
                return View("Flights", new FlightFilterViewModel());
            }
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Flights(FlightListViewModel model)
        //{
        //    try
        //    {
        //        return View(model );
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle errors
        //        ViewBag.ErrorMessage = "An error occurred while retrieving flights.";
        //        _logger.LogError(ex, "Error retrieving flights");
        //        return View("Flights", new FlightListViewModel());
        //    }
        //}

        private async Task<List<FlightSearchResultDto>> GetFilteredFlights(FlightFilterViewModel? model)
        {
            // Prepare the filter model
            var filterModel = new FlightFilterModel
            {
                DepartureAirportCode = model?.DepartureAirportCode,
                ArrivalAirportCode = model?.ArrivalAirportCode,
                DepartureDate = model?.DepartureDate,
                ArrivalDate = model?.ArrivalDate,
                AvailableSeatCount = model?.AvailableSeatCount
            };

            // Make HTTP request to API to get filtered flights
            var response = await _httpClient.PostAsJsonAsync(_getFilteredFlightsLink, filterModel);
            response.EnsureSuccessStatusCode(); // Ensure successful response
            var filteredFlights = await response.Content.ReadAsAsync<List<FlightSearchResultDto>>();

            return filteredFlights;
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
}
