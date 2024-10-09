namespace Foodies.Models
{
    public class Chat
    {
        [Key]
        public int ChatId { get; set; }
        public string AdminId { get; set; }
        public virtual Admin Admin { get; set; }
        public string CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual List<Message> Messages { get; set; } = new List<Message>();
    }
}
