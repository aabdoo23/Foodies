using Foodies.Common;
using Foodies.Data;

namespace Foodies.Repositories
{
    public class RestaurantRepository : IBaseRepository<Restaurant>
    {
        private readonly FoodiesDbContext _context;
        public RestaurantRepository(FoodiesDbContext context)
        {
            _context = context;
        }
        public async Task<Restaurant> Create(Restaurant entity)
        {
            await _context.Restaurants.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Restaurant> Delete(string id)
        {
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync(x => x.Id == id);
            if (restaurant == null)
            {
                return null;
            }
            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();
            return restaurant;
        }

        public async Task<IEnumerable<Restaurant>> GetAll()
        {
            return await _context.Restaurants.ToListAsync();
        }

        public async Task<Restaurant> GetById(string id)
        {
            return await _context.Restaurants.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Restaurant> Update(Restaurant entity)
        {
            _context.Restaurants.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
