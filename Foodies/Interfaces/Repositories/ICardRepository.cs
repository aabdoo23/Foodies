using Foodies.Common;

namespace Foodies.Interfaces.Repositories
{
    public interface ICardRepository : IBaseRepository<Card>
    {
        public Task<Card> GetCardByCustomerId(string customerId);


    }
}
