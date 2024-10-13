using Foodies.Common;

namespace Foodies.Interfaces.Repositories
{
    public interface IBranchRepository : IBaseRepository<Branch>
    {
        public Task<IEnumerable<Branch>> GetAllBrancheshByRestaurantId(string restaurantId);
    }
}
