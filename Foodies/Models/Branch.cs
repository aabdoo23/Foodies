using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foodies.Models
{
    public class Branch
    {
        public int BranchId { get; set; }
        public virtual Restaurant Restaurant { get; set; }
        public string BranchArea { get; set; }
        public string BranchLocation { get; set; } //link
        [Column(TypeName = "time")]
        public TimeSpan OpeningHour { get; set; }//0-23
        [Column(TypeName = "time")]
        public TimeSpan ClosingHour { get; set; }
    }
}
