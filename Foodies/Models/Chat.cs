using Foodies.Common;

namespace Foodies.Models
{
    public class Chat :BaseEntity
    {
        public virtual Restaurant Restaurant { get; set; } = default!;
        public virtual Customer Customer { get; set; } = default!;
        public virtual ICollection<Message> Messages { get; set; }

    }
}
