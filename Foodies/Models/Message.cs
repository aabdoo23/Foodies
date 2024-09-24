namespace Foodies.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string TimeStamp { get; set; } = string.Empty;

        public bool isCustomerSender { get; set; }
    }
}
