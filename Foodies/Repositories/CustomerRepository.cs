using Foodies.Common;
using Foodies.Data;
using Foodies.Exceptions;
using Foodies.Interfaces.Repositories;

namespace Foodies.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly FoodiesDbContext _context;
        public CustomerRepository(FoodiesDbContext context) {
            _context = context;
        }
        public async Task<Customer> Create(Customer entity)
        {
            _context.Customers.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Customer> Delete(string id)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(cus => cus.Id == id) ?? throw new NotFoundException($"Customer with ID {id} not found");
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<IEnumerable<Customer>> GetAll()
        {
            var customers = await _context.Customers.ToListAsync();
            return customers;
        }

        public async Task<Customer> GetById(string id)
        {
            var customer = await _context.Customers
                .Include(x => x.IdentityUser)
                .Include(x=>x.Address)
                .FirstOrDefaultAsync(x => x.Id == id) ?? throw new NotFoundException($"Customer with ID {id} not found");
            return customer;
        }

        public async Task<Customer> GetByIdWithFavouriteRestaurants(string? userId)
        {
            return await _context.Customers
                .Include(x=>x.IdentityUser)
                .Include(c => c.FavouriteRestaurants)
                .FirstOrDefaultAsync(x => x.Id == userId) ?? throw new NotFoundException($"Customer with ID {userId} not found");
        }
        public async Task<Customer> GetByIdIncludeOrders(string? userId)
        {
            return await _context.Customers
                .Include(o => o.Orders)
                .FirstOrDefaultAsync(x => x.Id == userId) ?? throw new NotFoundException($"Customer with ID {userId} not found");
        }
        public async Task<Customer> Update(Customer entity)
        {
            _context.Customers.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
