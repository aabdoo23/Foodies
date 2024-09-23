namespace Foodies.Models
{
    public class BranchManager
    {
        public int Id { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        public virtual Branch Branch { get; set; }
        public virtual Admin Admin { get; set; }

        public int BranchId { get; set; } // Foreign key added

    }
}
