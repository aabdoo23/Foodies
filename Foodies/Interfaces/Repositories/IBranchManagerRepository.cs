using Foodies.Common;

namespace Foodies.Interfaces.Repositories
{
    public interface IBranchManagerRepository : IBaseRepository<BranchManager>
    {
        Task<BranchManager> GetByIdWithBranchAndRestaurantIncluded(string id);
    }
}
