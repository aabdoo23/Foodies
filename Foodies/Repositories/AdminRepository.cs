using Foodies.Common;
using Foodies.Data;
using Foodies.Exceptions;
using Foodies.Interfaces.Repositories;

namespace Foodies.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly FoodiesDbContext _context;
        public AdminRepository(FoodiesDbContext context) {
            _context = context;
        }

        public async Task<Admin> Create(Admin entity)
        {
            _context.Admins.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Admin> Delete(string id)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(x => x.Id == id) ?? throw new NotFoundException($"Admin with {id} not found");
            _context.Admins.Remove(admin);
            await _context.SaveChangesAsync();
            return admin;
        }

        public Task<Admin> GetAdminByRestaurantId(string restaurantId)
        {
            var admin = _context.Admins.FirstOrDefaultAsync(x => x.RestaurantId == restaurantId)?? throw new NotFoundException($"Admin for restaurantId {restaurantId} not found");
            return admin;
        }

        public async Task<IEnumerable<Admin>> GetAll()
        {
            var admins = await _context.Admins.ToListAsync();
            return admins;
        }

        public async Task<Admin> GetById(string id)
        {
            var admin = await _context.Admins
                .Include(x => x.IdentityUser)
                .Include(x => x.Restaurant)
                .FirstOrDefaultAsync(x => x.Id == id);

            return admin ?? throw new NotFoundException($"Admin with {id} or includes not found");
        }

        public async Task<Admin> Update(Admin entity)
        {
            _context.Admins.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
