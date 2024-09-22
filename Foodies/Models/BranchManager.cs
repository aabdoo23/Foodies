namespace Foodies.Models
{
    public class BranchManager
    {
        public int Id { get; set; }

        public string username { get; set; }
        public string password { get; set; }

        public virtual Branch Branch { get; set; }
        public virtual Admin Admin { get; set; }

        public int BranchId { get; set; } // Foreign key added

    }
}
