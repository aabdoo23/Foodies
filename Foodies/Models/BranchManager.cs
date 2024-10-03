namespace Foodies.Models
{
    public class BranchManager : BaseUser
    {
        public int BranchId { get; set; }
        public virtual Branch Branch { get; set; }
        public string AdminId { get; set; }
        public virtual Admin Admin { get; set; }

    }
}
