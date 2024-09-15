using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Foodies.Models
{
	public class Customer
	{
		#region nonrelational
		public int CustomerId {  get; set; }
		[Required()]
		public string CustomerFirstName { get; set; }
		[Required()]
		public string CustomerLastName { get; set; }
		[Required()]
		public string CustomerEmail { get; set; }
		[Required()]
		public string Password { get; set; }
		[Required()]
		public string City { get; set; }
		[AllowNull]
		public string Street {  get; set; }
		[AllowNull]
		public string Building { get; set; }

		public int Points { get; set; } = 0;
		#endregion

		public virtual List<Restaurant>? FavouriteRestaurants {  get; set; }
		public virtual List<Order>? Orders { get; set; }
		//The ratin 
		public virtual Cart? CusCart { get; set; }

		public virtual List<Rating>? ratedresturants { get; set; }


	}
}
