namespace Foodies.Models
{
    public class Payment
    {

        public int Id { get; set; }
        public string PaymentMethod { get; set; }
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }
        [Column(TypeName = "DATETIME")]
        public DateTime PaymentDate { get; set; }
        public virtual Order Order { get; set; } = default!;

        public int ?cardId { get; set; }
        public virtual Card ?card { get; set; }

    }

}
