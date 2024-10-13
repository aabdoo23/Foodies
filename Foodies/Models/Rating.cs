using Foodies.Common;

namespace Foodies.Models
{
    public class Rating: BaseEntity
    {
        public string CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }
        public string RestaurantId { get; set; }
        public virtual Restaurant? Restaurant { get; set; }
        [Column(TypeName = "decimal(5, 2)")]
        public decimal? Rate { get; set; }


    }

}
