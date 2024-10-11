namespace Foodies.Models
{

    public class Customer : BaseUser
    {
        public int Points { get; set; } = 0;
        public string AddressId { get; set; }
        public virtual Address Address { get; set; }
        [NotMapped]
        public virtual List<Restaurant>? FavouriteRestaurants { get; set; }
        public virtual List<Order>? Orders { get; set; }
        public virtual List<Rating>? Ratings { get; set; }

    }
}
