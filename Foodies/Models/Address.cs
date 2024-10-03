namespace Foodies.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string? Street { get; set; }
        public string? Building { get; set; }
        public string? Location { get; set; }
        public string? CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }
        public int? BranchId { get; set; }
        public virtual Branch? Branch { get; set; }
    }

}
