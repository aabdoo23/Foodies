using Foodies.Common;

namespace Foodies.Interfaces.Repositories
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        public Task<IEnumerable<Order>> GetOrdersByBranchIdWithItems(string branchId);
        public Task<Order> GetByIdWithBranchIncluded(string id);
        public  Task<Order> GetBycustomeridwithMenu(string cusid);
    }
}
