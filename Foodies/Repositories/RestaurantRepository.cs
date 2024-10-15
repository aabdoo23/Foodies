using Foodies.Common;
using Foodies.Data;
using Foodies.Exceptions;
using Foodies.Interfaces.Repositories;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace Foodies.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
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
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync(x => x.Id == id) ?? throw new NotFoundException($"Restaurant with {id} not found");
            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();
            return restaurant;
        }

        public async Task<IEnumerable<Restaurant>> GetAll()
        {
            return await _context.Restaurants.ToListAsync();
        }

        public async Task<Restaurant> GetByIdWithMenuItems(string id)
        {
            return await _context.Restaurants
                .Include(x => x.MenuItems)
                .FirstOrDefaultAsync(x => x.Id == id) ?? throw new NotFoundException($"Restaurant with {id} not found");
        }

        public async Task<Restaurant> GetByIdWithMenuItemsAndBranches(string id)
        {
            return await _context.Restaurants
                .Include(x => x.MenuItems)
                .Include(x => x.Branches)
                .FirstOrDefaultAsync(x => x.Id == id) ?? throw new NotFoundException($"Restaurant with {id} not found");
        }

        public async Task<Restaurant> GetByIdWithFavouriteCustomers(string id)
        {
            return await _context.Restaurants
                .Include(x => x.FavouriteCustomers)
                .FirstOrDefaultAsync(x => x.Id == id) ?? throw new NotFoundException($"Restaurant with {id} not found");
        }
        public async Task<Restaurant> GetById(string id)
        {
            return await _context.Restaurants.FirstOrDefaultAsync(x => x.Id == id) ?? throw new NotFoundException($"Restaurant with {id} not found");
        }

        public async Task<Restaurant> Update(Restaurant entity)
        {
            _context.Restaurants.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Restaurant> GetByIdWithRatings(string id)
        {
            return await _context.Restaurants
                .Include(x => x.Ratings)
                .FirstOrDefaultAsync(x => x.Id == id) ?? throw new NotFoundException($"Restaurant with {id} not found");
        }
        public async Task<Restaurant> GetByIdWithBranchesIncludeAddress(string id)
        {
            return await _context.Restaurants
                .Where(x => x.Id == id)
                .Include(b => b.Branches)
                .ThenInclude(b => b.Address)
                .FirstOrDefaultAsync() ?? throw new NotFoundException($"Restaurant with {id} not found");
        }

    }


}
