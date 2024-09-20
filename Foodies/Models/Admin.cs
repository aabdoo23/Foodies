namespace Foodies.Models
{

    public class Admin
    {
        public int AdminId { get; set; }
        public string AdminFirstName { get; set; } = string.Empty;
        public string AdminLastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public virtual Restaurant Restaurant { get; set; } = default!;
        public int RestaurantId { get; set; }
    }

}
