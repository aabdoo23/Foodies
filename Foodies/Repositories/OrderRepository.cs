using Foodies.Common;
using Foodies.Data;
using Foodies.Exceptions;
using Foodies.Interfaces.Repositories;

namespace Foodies.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly FoodiesDbContext _context;
        public OrderRepository(FoodiesDbContext context)
        {
            _context = context;
        }
        public async Task<Order> Create(Order entity)
        {
            await _context.Orders.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Order> Delete(string id)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x=>x.Id==id) ?? throw new NotFoundException($"Order with ID {id} not found");
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<IEnumerable<Order>> GetAll()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<Order> GetById(string id)
        {
            return await _context.Orders.FirstOrDefaultAsync(x => x.Id == id) ?? throw new NotFoundException($"Order with ID {id} not found");
        }

        public async Task<Order> Update(Order entity)
        {
            _context.Orders.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
