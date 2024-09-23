using System.ComponentModel.DataAnnotations.Schema;

namespace Foodies.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public virtual Customer? Customer { get; set; }
        public virtual Restaurant? Restaurant { get; set; }
        [Column(TypeName = "decimal(5, 2)")]
        public decimal? Rate { get; set; }

    }

}
