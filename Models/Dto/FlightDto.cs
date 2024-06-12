namespace AirlineTicketingSystemWebApp.Models.Dto
{
    public class FlightDto
    {
        public string Code { get; set; }  
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public string DepartureAirportCode { get; set; }
        public string ArrivalAirportCode { get; set; }
        public string PlaneCode { get; set; }
    }

    public class FlightSearchResultDto
    {
        public string Code { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public string DepartureAirportCode { get; set; }
        public string ArrivalAirportCode { get; set; }
        public string PlaneCode { get; set; }
        public DateTime DateCreated { get; set; }
        public double TotalMiles { get; set; }
        public int TotalMinutes { get; set; }
        public int AvailableSeatCount { get; set; }
        public List<int> OccupiedSeats { get; set; } = new List<int>();
    }
}
