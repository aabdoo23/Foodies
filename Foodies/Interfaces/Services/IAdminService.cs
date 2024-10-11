using Foodies.ViewModels;

namespace Foodies.Interfaces.Services
{
    public interface IAdminService
    {
        public Task<Admin> CreateAdmin(AdminRegisterViewModel viewModel);
    }
}
