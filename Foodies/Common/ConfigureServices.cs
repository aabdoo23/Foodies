using Foodies.Interfaces.Repositories;
using Foodies.Interfaces.Services;
using Foodies.Repositories;
using Foodies.Services;

namespace Foodies.Common
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IBranchManagerRepository, BranchManagerRepository>();
            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IMenuItemRepository, MenuItemRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<IRestaurantRepository, RestaurantRepository>();
            services.AddScoped<ICardRepository, CardRepository>();


            return services;
        }
        public static IServiceCollection AddServiceInjection(this IServiceCollection services)
        {
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IBranchManagerService, BranchManagerService>();

            return services;
        }
    }
}
