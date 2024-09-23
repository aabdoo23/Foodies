using System.ComponentModel.DataAnnotations;

namespace Foodies.Models
{

    public class Admin
    {
        public int AdminId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; } 
        public string Email { get; set; }
        public string? img { get; set; }

        public string Password { get; set; }
        public virtual Restaurant Restaurant { get; set; }
        public int RestaurantId { get; set; }

        public virtual List<BranchManager>? BranchManagers { get; set; }

    }

}
