namespace Foodies.Models
{
	public class Cart
	{
		public int CartId { get; set; }
		public int Quantity { get; set; }	
		public virtual List<MenuItem>? Items { get; set; }
		public virtual Order Order { get; set; }
	}
}
