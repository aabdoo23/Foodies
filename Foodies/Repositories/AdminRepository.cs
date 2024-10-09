using Foodies.Common;
using Foodies.Data;

namespace Foodies.Repositories
{
    public class AdminRepository : IBaseRepository<Admin>
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
            var admin = await _context.Admins.FirstOrDefaultAsync(x => x.Id == id);
            _context.Admins.Remove(admin);
            await _context.SaveChangesAsync();
            return admin;
        }

        public async Task<IEnumerable<Admin>> GetAll()
        {
            var admins = await _context.Admins.ToListAsync();
            return admins;
        }

        public async Task<Admin> GetById(string id)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(x => x.Id == id);
            return admin;
        }

        public async Task<Admin> Update(Admin entity)
        {
            _context.Admins.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
