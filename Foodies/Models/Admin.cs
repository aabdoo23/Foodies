namespace Foodies.Models
{
    public class Admin:BaseUser
    {
        public int RestaurantId { get; set; }
        public virtual Restaurant Restaurant { get; set; }
        public virtual List<BranchManager>? BranchManagers { get; set; }

    }
}
