using Foodies.Common;
using Microsoft.AspNetCore.Identity;

namespace Foodies.Models
{
    public  class BaseUser : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? img { get; set; }
        public virtual IdentityUser IdentityUser { get; set; }
    }
}
