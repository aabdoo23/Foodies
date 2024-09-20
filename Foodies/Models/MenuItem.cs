namespace Foodies.Models
{
    public class MenuItem
    {
        public int MenuItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Price { get; set; }
        public string Description { get; set; } = string.Empty;

        public Restaurant Resturant { get; set; } = default!;
        public virtual ICollection<Order>? Orders { get; set; }

    }

}
