using System.ComponentModel.DataAnnotations.Schema;

namespace Foodies.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public string PaymentMethod { get; set; }
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }
        [Column(TypeName = "DATETIME")]
        public DateTime PaymentDate { get; set; }
        public virtual Order Order { get; set; }
    }
}
