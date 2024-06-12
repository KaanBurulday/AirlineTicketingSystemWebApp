using AirlineTicketingSystemWebApp.Models.Dto;

namespace AirlineTicketingSystemWebApp.Models
{
    public class FlightFilterViewModel
    {
        public string? DepartureAirportCode { get; set; }
        public string? ArrivalAirportCode { get; set; }
        public DateTime? DepartureDate { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public int? AvailableSeatCount { get; set; }
        public List<FlightSearchResultDto> flightDtos { get; set; } = new List<FlightSearchResultDto>();
    }
}
