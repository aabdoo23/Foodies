using Foodies.Common;
using Foodies.Data;

namespace Foodies.Repositories
{
    public class PaymentRepository : IBaseRepository<Payment>
    {
        private readonly FoodiesDbContext _context;
        public PaymentRepository(FoodiesDbContext context)
        {
            _context = context;
        }
        public async Task<Payment> Create(Payment entity)
        {
            await _context.Payments.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Payment> Delete(string id)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(x => x.Id == id);
            if (payment == null)
            {
                return null;
            }
            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<IEnumerable<Payment>> GetAll()
        {
            return await _context.Payments.ToListAsync();
        }

        public async Task<Payment> GetById(string id)
        {
            return await _context.Payments.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Payment> Update(Payment entity)
        {
            _context.Payments.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
