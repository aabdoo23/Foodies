﻿using Foodies.Common;

namespace Foodies.Interfaces.Repositories
{
    public interface IRatingRepository : IBaseRepository<Rating>
    {
        public Task<IEnumerable<Rating>> GetAllByRestaurantId(string restaurantId);
        public Task<Rating> GetByCustomerIdAndRestaurantId(string customerId, string restaurantId);
    }
}
