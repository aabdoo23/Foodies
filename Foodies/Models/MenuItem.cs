using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Foodies.Models
{
    public class MenuItem
    {

        [Key]
        public int MenuItemId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Category { get; set; }
        public string? img { get; set; }

        public string Description { get; set; }

        public Restaurant Resturant { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }

    }

}
