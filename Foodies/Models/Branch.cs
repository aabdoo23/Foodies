namespace Foodies.Models
{
    public class Branch
    {
        public int BranchId { get; set; }
        public virtual Restaurant Restaurant { get; set; } = default!; // rest id 
        [Column(TypeName = "time")]
        public TimeSpan OpeningHour { get; set; }//0-23
        [Column(TypeName = "time")]
        public TimeSpan ClosingHour { get; set; }
        public virtual List<Order>? Orders { get; set; }
        public virtual BranchManager BranchManager { get; set; } 
        public int AddressId { get; set; }
        public virtual Address? Address { get; set; }

    }

}
