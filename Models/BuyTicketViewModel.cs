using System.ComponentModel.DataAnnotations;

namespace AirlineTicketingSystemWebApp.Models
{
    public class BuyTicketViewModel
    {
        [Required(ErrorMessage = "Number of passenger entry is required")]
        public int NumberOfPassengers { get; set; }
        [Required(ErrorMessage = "MilesPoint usage selection is required")]
        public bool BuyWithMilesPoints { get; set; }
        [Required(ErrorMessage = "Seat selection is required")]
        public List<int> SeatNumbers { get; set; } = new List<int>();

        public List<int> AvailableSeatNumbers { get; set; } = new List<int>();
        public List<int> OccupiedSeatNumbers { get; set; } = new List<int>();
        public string FlightCode { get; set; }

        public string? UserId { get; set; }
    }
}
