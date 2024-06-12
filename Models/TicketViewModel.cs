using AirlineTicketingSystemWebApp.Models.Dto;

namespace AirlineTicketingSystemWebApp.Models
{
    public class TicketViewModel
    {
        public List<TicketDto> Tickets { get; set; } = new List<TicketDto>();
    }
}
