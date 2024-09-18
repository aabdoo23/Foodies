using System.ComponentModel.DataAnnotations.Schema;

namespace Foodies.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string OrderState { get; set; }
        [Column(TypeName = "money")]
        public decimal TotalPrice { get; set; }
        [Column(TypeName = "DATETIME")]
        public DateTime OrderDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Restaurant Restaurant { get; set; }
        public virtual Payment Payment { get; set; }

        //-----------------------------------
        public int PaymentId { get; set; } // Foreign key added


    }
}
