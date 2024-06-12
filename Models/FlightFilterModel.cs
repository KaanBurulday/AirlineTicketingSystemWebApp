namespace AirlineTicketingSystemWebApp.Models
{
    public class FlightFilterModel
    {
        public string? DepartureAirportCode { get; set; }
        public string? ArrivalAirportCode { get; set; }
        public DateTime? DepartureDate { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public int? AvailableSeatCount { get; set; }
        public QueryWithPagingDto Query { get; set; } = new QueryWithPagingDto();
    }

    public class QueryWithPagingDto
    {
        const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }
}
