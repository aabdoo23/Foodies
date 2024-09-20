namespace Foodies.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string OrderState { get; set; } = string.Empty;
        [Column(TypeName = "money")]
        public decimal TotalPrice { get; set; }
        [Column(TypeName = "DATETIME")]
        public DateTime OrderDate { get; set; }

        public virtual Customer Customer { get; set; } = default!;
        public virtual Restaurant Restaurant { get; set; } = default!;
        public virtual Payment Payment { get; set; } = default!;

        public int PaymentId { get; set; } // Foreign key added
        public virtual ICollection<MenuItem> Items { get; set; }


    }

}
