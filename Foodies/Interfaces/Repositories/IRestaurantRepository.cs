using Foodies.Common;

namespace Foodies.Interfaces.Repositories
{
    public interface IRestaurantRepository : IBaseRepository<Restaurant>
    {
        public Task<Restaurant> GetByIdWithMenuItems(string id);
        public Task<Restaurant> GetByIdWithMenuItemsAndBranches(string id);

    }
}
