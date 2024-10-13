using Foodies.Common;
using Foodies.Data;
using Foodies.Exceptions;
using Foodies.Interfaces.Repositories;

namespace Foodies.Repositories
{
    public class PaymentRepository : IPaymentRepository
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
            var payment = await _context.Payments.FirstOrDefaultAsync(x => x.Id == id) ?? throw new NotFoundException($"Payment with {id} not found");
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
            return await _context.Payments.FirstOrDefaultAsync(x => x.Id == id) ?? throw new NotFoundException($"Payment with {id} not found");
        }

        public async Task<Payment> Update(Payment entity)
        {
            _context.Payments.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
