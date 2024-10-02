using Microsoft.AspNetCore.Identity;

namespace Foodies.Models
{
    public class BaseUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? img { get; set; }
    }
}
