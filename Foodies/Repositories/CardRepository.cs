using Foodies.Data;
using Foodies.Exceptions;
using Foodies.Interfaces.Repositories;
using GoogleApi.Entities.Search.Video.Common;

namespace Foodies.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly FoodiesDbContext _context;
        public CardRepository(FoodiesDbContext context)
        {
            _context = context;
        }

        public async Task<Card> Create(Card entity)
        {
            _context.Cards.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Card> Delete(string id)
        {
            var card = await _context.Cards.FirstOrDefaultAsync(c => c.Id == id) ?? throw new NotFoundException($"Card with ID {id} not found");
            _context.Cards.Remove(card);
            await _context.SaveChangesAsync();
            return card;
        }

        

        public async Task<Card> GetCardByCustomerId(string customerId)
        {
            var card = await _context.Cards
            .Where(c => c.CustomerId == customerId)
            .Include(c => c.payments)
            .FirstOrDefaultAsync() ?? throw new NotFoundException($"Card with Customer Id = {customerId} not found");
            return card;
        }

        public async Task<Card> GetById(string id)
        {
            var Card = await _context.Cards
                .Include(a => a.payments)
                .FirstOrDefaultAsync(x => x.Id == id) ?? throw new NotFoundException($"Card with ID {id} not found");
            return Card;
        }

        public async Task<Card> Update(Card entity)
        {
            _context.Cards.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<Card>> GetAll()
        {
            var cards = await _context.Cards.ToListAsync();
            return cards;
        }
    }
}
