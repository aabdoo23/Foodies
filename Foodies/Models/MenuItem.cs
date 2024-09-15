using System.ComponentModel.DataAnnotations;

namespace Foodies.Models
{
	public class MenuItem
	{
      public int MenuItemId { get; set; }
	  public string Name { get; set; }
	  public int Price { get; set; }
	  public string Description { get; set; }
    
	  [Required]
	  public Restaurant resturant { get; set; }
	  
	}
}
