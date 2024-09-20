namespace Foodies.Models
{
    public class Branch
    {
        public int BranchId { get; set; }
        public virtual Restaurant Restaurant { get; set; } = default!;
        public string BranchArea { get; set; } = string.Empty;
        public string BranchLocation { get; set; } = string.Empty; //link
        [Column(TypeName = "time")]
        public TimeSpan OpeningHour { get; set; }//0-23
        [Column(TypeName = "time")]
        public TimeSpan ClosingHour { get; set; }
    }

}
