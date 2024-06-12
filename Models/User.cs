using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirlineTicketingSystemWebApp.Model
{
    [Table("User")]
    public class User : IdentityUser
    {
        [Required] 
        public required string Name { get; set; }
        [Required] 
        public required string Surname { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        public required string UserRole { get; set; }

        // Additional Information - Not Necessary
        public DateTime? Birthdate { get; set; }
        public string? Nation { get; set; }
        public string? PassportNumber { get; set; }
    }
}
