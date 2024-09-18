using System.ComponentModel.DataAnnotations;

namespace Foodies.Models
{
    public class Admin
    {
        public int AdminId { get; set; }
        [Required()]
        public string AdminFirstName { get; set; }
        [Required()]
        public string AdminLastName { get; set; }
        [Required()]
        public string PhoneNumber { get; set; }
        [Required()]
        public string Email { get; set; }
        [Required()]
        public string Password { get; set; }
        public virtual Restaurant Restaurant { get; set; }
        public int RestaurantId { get; set; }
    }
}
