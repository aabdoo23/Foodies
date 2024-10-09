namespace Foodies.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        public int ChatId { get; set; }
        public virtual Chat Chat { get; set; }
        public string Content { get; set; }
        public string SenderId { get; set; }
        public DateTime SentAt { get; set; } = DateTime.Now;
        public bool IsRead { get; set; } = false;
    }
}
