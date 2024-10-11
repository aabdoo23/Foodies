namespace Foodies.Models
{
    public class MenuItem
    {


        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Category { get; set; }
        public string? img { get; set; }

        public int? Quantity { get; set; } = 1;
        public string Description { get; set; }

        public Restaurant Resturant { get; set; } = default!;

        public virtual ICollection<Order>? Orders { get; set; }

    }

}
