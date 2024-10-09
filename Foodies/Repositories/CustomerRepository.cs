using Foodies.Common;
using Foodies.Data;

namespace Foodies.Repositories
{
    public class CustomerRepository : IBaseRepository<Customer>
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
            var customer = await _context.Customers.FirstOrDefaultAsync(cus => cus.Id == id);
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
            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);
            return customer;
        }

        public async Task<Customer> Update(Customer entity)
        {
            _context.Customers.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
