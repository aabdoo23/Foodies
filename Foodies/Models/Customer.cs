namespace Foodies.Models
{

    public class Customer : BaseUser

    {
        public int Points { get; set; } = 0;
        public int AddressId { get; set; }
        public virtual Address Address { get; set; }
        public virtual Card card { get; set; }

        [NotMapped]
        public virtual List<Restaurant>? FavouriteRestaurants { get; set; }
        public virtual List<Order>? Orders { get; set; }
        //The ratin
        //yo bro
        //we ratin

        public virtual List<Rating>? Ratings { get; set; }

    }
}
