using System.ComponentModel.DataAnnotations;

namespace AirlineTicketingSystemWebApp.Models
{
    public class AddFlightViewModel
    {
        [Required(ErrorMessage = "Flight code is required")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Departure date is required")]
        public DateTime DepartureDate { get; set; }
        [Required(ErrorMessage = "Arrival date is required")]
        public string DepartureAirportCode { get; set; }
        [Required(ErrorMessage = "Arrival airport code is required")]
        public string ArrivalAirportCode { get; set; }
        [Required(ErrorMessage = "Plane code selection is required")]
        public string PlaneCode { get; set; }
    }
}
