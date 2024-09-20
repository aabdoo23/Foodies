namespace Foodies.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }
        [Column(TypeName = "DATETIME")]
        public DateTime PaymentDate { get; set; }
        public virtual Order Order { get; set; } = default!;
    }

}
