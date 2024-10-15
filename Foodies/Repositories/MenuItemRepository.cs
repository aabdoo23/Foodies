using Foodies.Common;
using Foodies.Data;
using Foodies.Exceptions;
using Foodies.Interfaces.Repositories;

namespace Foodies.Repositories
{
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly FoodiesDbContext _context;
        public MenuItemRepository(FoodiesDbContext context)
        {
            _context = context;
        }
        public async Task<MenuItem> Create(MenuItem entity)
        {
            await _context.MenuItems.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<MenuItem> Delete(string id)
        {
            var menuItem = await _context.MenuItems.FindAsync(id) ?? throw new NotFoundException($"MenuItem with ID {id} not found");
            _context.MenuItems.Remove(menuItem);
            await _context.SaveChangesAsync();
            return menuItem;
        }

        public async Task<IEnumerable<MenuItem>> GetAll()
        {
            return await _context.MenuItems.ToListAsync();
        }

        public async Task<MenuItem> GetById(string id)
        {
            var menuItem = await _context.MenuItems.Include(x => x.Resturant).FirstOrDefaultAsync(x => x.Id == id) ?? throw new NotFoundException($"MenuItem with ID {id} not found");
            return menuItem;
        }

        public async Task<MenuItem> Update(MenuItem entity)
        {
            _context.MenuItems.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<MenuItem> GetByIdWithOrders(string id)
        {
            return await _context.MenuItems
                .Include(x => x.Orders)
                .FirstOrDefaultAsync(x => x.Id == id) ?? throw new NotFoundException($"Menuitem with {id} not found");
        }
        public async Task<IEnumerable<MenuItem>> GetAllByRestaurantId(string restaurantId)
        {
            return await _context.MenuItems.Where(x => x.Resturant.Id == restaurantId).ToListAsync();
        }

        public async Task<IEnumerable<MenuItem>> GetAllByRestaurantId(string restaurantId, string category)
        {
            if(category == null || category=="")
                return await _context.MenuItems.Where(x => x.Resturant.Id == restaurantId).ToListAsync();
            return await _context.MenuItems.Where(x => x.Resturant.Id == restaurantId && x.Category == category).ToListAsync();
        }
    }
}
