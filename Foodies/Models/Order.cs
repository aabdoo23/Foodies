namespace Foodies.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public string State { get; set; }
        [Column(TypeName = "money")]
        public decimal TotalPrice { get; set; }
        [Column(TypeName = "DATETIME")]
        public DateTime OrderDate { get; set; }

        public virtual Customer Customer { get; set; } = default!;
        public virtual Payment? Payment { get; set; } = default!;
        public int? PaymentId { get; set; } // Foreign key added
        public virtual List<MenuItem> Items { get; set; }


    

        //public int BranchId { get; set; } // Foreign key added
        public virtual Branch Branch { get; set; }


    }

}
