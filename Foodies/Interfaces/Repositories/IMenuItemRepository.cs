using Foodies.Common;
using Foodies.Data;

namespace Foodies.Interfaces.Repositories
{
    public interface IMenuItemRepository : IBaseRepository<MenuItem>
    {
        public Task<IEnumerable<MenuItem>> GetAllByRestaurantId(string id);
    }
}
