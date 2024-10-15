using Foodies.Common;

namespace Foodies.Models
{
    public class Message : BaseEntity
    {
        public string Content { get; set; } = string.Empty;
        public string TimeStamp { get; set; } = string.Empty;

        public bool isCustomerSender { get; set; }
    }
}
