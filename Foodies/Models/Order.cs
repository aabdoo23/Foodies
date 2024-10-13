using Foodies.Common;

namespace Foodies.Models
{
    public class Order :BaseEntity
    {
        public string State { get; set; }
        [Column(TypeName = "money")]
        public decimal TotalPrice { get; set; }
        [Column(TypeName = "DATETIME")]
        public DateTime OrderDate { get; set; }

        public virtual Customer Customer { get; set; } = default!;
   
        public virtual Payment? Payment { get; set; } = default!;

        public string? PaymentId { get; set; } // Foreign key added
        public virtual List<MenuItem> Items { get; set; }

        public virtual Branch Branch { get; set; }


    }

}
