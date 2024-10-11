using Foodies.Common;
using Foodies.Data;

namespace Foodies.Interfaces.Repositories
{
    public interface IAdminRepository : IBaseRepository<Admin>
    {
        public Task<Admin> GetAdminByRestaurantId(string restaurantId);
    }
}
