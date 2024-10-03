using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Foodies
{
    public static class UserRoles
    {
        //public const string Admin = "Admin";
        //public const string BranchManager = "BranchManager";
        //public const string Customer = "Customer";

        // Static method to create the Admin role
        public static async Task<IdentityResult> CreateRole(RoleManager<IdentityRole> roleManager, string who)
        {
            IdentityRole role = new IdentityRole(who);
            IdentityResult result = await roleManager.CreateAsync(role);
            return result;
        }
    }
}
