using Foodies.Common;
using Foodies.Exceptions;
using Foodies.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Foodies.Services
{
    public class AdminService
    {
        private readonly IBaseRepository<Admin> _adminRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AdminService(IBaseRepository<Admin> adminRepository,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _adminRepository = adminRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<Admin> CreateAdmin(AdminRegisterViewModel viewModel)
        {
            var existingAdmin = await _userManager.FindByEmailAsync(viewModel.Email);
            if (existingAdmin != null)
            {
                throw new UserAlreadyExistsException(viewModel.Email);
            }
            IdentityUser user = new IdentityUser();
            user.UserName = viewModel.Email;
            user.Email = viewModel.Email;
            user.PhoneNumber = viewModel.phoneNumber;

            IdentityResult result = await _userManager.CreateAsync(user, viewModel.Password);
            if (result.Succeeded)
            {
                _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin)).GetAwaiter().GetResult();
                await _userManager.AddToRoleAsync(user, "Admin");
                Admin admin = new Admin
                {
                    Id = user.Id,
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    IdentityUser = user,
                };
                await _adminRepository.Create(admin);
                return admin;
            }
            return null;
        }
    }
}
