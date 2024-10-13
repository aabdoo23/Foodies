using Foodies.Common;

namespace Foodies.Models
{
    public class MenuItem : BaseEntity
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public string Category { get; set; }
        public string? img { get; set; }

        //public int? Quantity { get; set; } = 1;
        public string Description { get; set; }

        public virtual Restaurant Resturant { get; set; } = default!;
        public string ResturantId { get; set; } = default!;


        public virtual ICollection<Order>? Orders { get; set; }

    }

}
