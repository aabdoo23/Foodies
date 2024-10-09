namespace Foodies.Common
{
    public class BaseEntity
    {
        [Key]
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
