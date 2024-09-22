using System.ComponentModel.DataAnnotations;
namespace Foodies.Models
{
    public class Message
    {
        [Required]
        public int Id { get; set; }
        public string Content { get; set; }
        public string TimeStamp { get; set; }

        public bool isCustomerSender { get; set; }
        

    }
}
