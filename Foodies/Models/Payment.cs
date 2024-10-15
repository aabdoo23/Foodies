using Foodies.Common;

namespace Foodies.Models
{
    public class Payment: BaseEntity
    {
        public string PaymentMethod { get; set; }
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }
        [Column(TypeName = "DATETIME")]
        public DateTime PaymentDate { get; set; }
        public virtual Order Order { get; set; } = default!;

        public string ?cardId { get; set; }
        public virtual Card ?card { get; set; }

    }

}
