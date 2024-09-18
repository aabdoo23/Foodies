namespace Foodies.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public virtual Restaurant Restaurant { get; set; }
        public virtual Customer Customer{ get; set; }
        public virtual ICollection<Message> Messages { get; set; }

    }
}
