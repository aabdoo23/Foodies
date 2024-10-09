using Foodies.Common;

namespace Foodies.Models
{
    public class MenuItem : BaseEntity
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public string Category { get; set; }
        public string? img { get; set; }

        public string Description { get; set; }

        public Restaurant Resturant { get; set; } = default!;
        public virtual ICollection<Order>? Orders { get; set; }

    }

}
