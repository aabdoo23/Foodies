namespace Foodies.Models
{
	public class Rating
	{
		public int RatingId { get; set; }	
		public virtual Customer? Customer { get; set; }
		public virtual Restaurant? Restaurant { get; set; }
		public decimal? rate { get; set; }
       
	}
}
//id 
//idcustomr ,kl==
