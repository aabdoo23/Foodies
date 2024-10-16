using Foodies.Common;

namespace Foodies.Models
{
    public class Card : BaseEntity
    {
        public string? CardNumber { get; set; }
        public string? ExpiryMonth { get; set; }
        public string? ExpiryYear { get; set; }
        public string? CVC { get; set; }

        public string? Type { get; set; }
        public Customer customer { get; set; }
        public string CustomerId { get; set; } //foriegn

        public virtual List<Payment> ?payments { get; set; }
    }
}
