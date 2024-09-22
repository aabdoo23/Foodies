using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Foodies.Models
{

    public class Customer
    {
        public int CustomerId { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerEmail { get; set; }
        public String PhoneNumber { get; set; } 

		public string Password { get; set; }

        public string City { get; set; }
        public string? Street { get; set; }
        public string? Building { get; set; }
        public int Points { get; set; } = 0;

        [NotMapped]
        public virtual List<Restaurant>? FavouriteRestaurants { get; set; }
        public virtual List<Order>? Orders { get; set; }
        //The ratin
        public virtual List<Rating>? ratedresturants { get; set; }

    }

}
