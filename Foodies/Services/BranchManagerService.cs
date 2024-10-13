using Foodies.Common;
using Foodies.Exceptions;
using Foodies.Interfaces.Repositories;
using Foodies.Interfaces.Services;
using Foodies.Repositories;
using Foodies.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Foodies.Services
{
    public class BranchManagerService : IBranchManagerService
    {
        private readonly IBranchManagerRepository _branchManagerRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public BranchManagerService(IBranchManagerRepository branchmanager, 
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _branchManagerRepository = branchmanager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<BranchManager> CreateBranchManager(AddbranchViewmodel viewModel, Admin admin, Branch branch)
        {
            var existingCustomer = await _userManager.FindByEmailAsync(viewModel.Email);
            if (existingCustomer != null)
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
                _roleManager.CreateAsync(new IdentityRole(UserRoles.BranchManager)).GetAwaiter().GetResult();
                await _userManager.AddToRoleAsync(user, "BranchManager");
                BranchManager branchManager = new BranchManager
                {
                    Id = user.Id,
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    Admin = admin,
                    Branch = branch,
                    IdentityUser = user,
                };
                await _branchManagerRepository.Create(branchManager);
                return branchManager;
            }
            return null;
        }
    }
}
