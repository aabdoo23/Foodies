using Foodies.Common;
using Foodies.Exceptions;
using Foodies.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Foodies.Interfaces.Services
{
    public interface ICustomerService
    {
        public Task<Customer> CreateCustomer(RegistrationViewModel viewModel);
    }
}
