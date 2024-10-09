using Foodies.Common;
using Foodies.Data;

namespace Foodies.Repositories
{
    public class MenuItemRepository : IBaseRepository<MenuItem>
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
            var menuItem = await _context.MenuItems.FindAsync(id);
            if (menuItem == null)
            {
                return null;
            }
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
            var menuItem = await _context.MenuItems.FirstOrDefaultAsync(x => x.Id == id);
            return menuItem;
        }

        public async Task<MenuItem> Update(MenuItem entity)
        {
            _context.MenuItems.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<MenuItem>> GetAllByRestaurantId(string id)
        {
            return await _context.MenuItems.Where(x => x.Resturant.Id == id).ToListAsync();
        }
    }
}
