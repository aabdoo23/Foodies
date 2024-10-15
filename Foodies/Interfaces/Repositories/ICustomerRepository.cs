using Foodies.Common;
using Foodies.Data;

namespace Foodies.Interfaces.Repositories
{
    public interface ICustomerRepository : IBaseRepository<Customer>
    {
        public Task<Customer> GetByIdWithFavouriteRestaurants(string? userId);
        public Task<Customer> GetByIdIncludeOrders(string? userId);

    }
}
