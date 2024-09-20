namespace Foodies.Models
{

    public class Customer
    {
        public int CustomerId { get; set; }
        public string CustomerFirstName { get; set; } = string.Empty;
        public string CustomerLastName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string Building { get; set; } = string.Empty;

        public int Points { get; set; } = 0;

        [NotMapped]
        public virtual List<Restaurant>? FavouriteRestaurants { get; set; }
        public virtual List<Order>? Orders { get; set; }
        //The ratin
        public virtual List<Rating>? RatedResturants { get; set; }

    }

}
