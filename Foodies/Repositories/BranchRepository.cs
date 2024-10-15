using Foodies.Common;
using Foodies.Data;
using Foodies.Exceptions;
using Foodies.Interfaces.Repositories;

namespace Foodies.Repositories
{
    public class BranchRepository : IBranchRepository
    {
        private readonly FoodiesDbContext _context;
        public BranchRepository(FoodiesDbContext context)
        {
            _context = context;
        }

        public async Task<Branch> Create(Branch entity)
        {
            _context.Branches.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Branch> Delete(string id)
        {
            var branch = await _context.Branches.FirstOrDefaultAsync(br => br.Id == id) ?? throw new NotFoundException($"Branch with ID {id} not found");
            _context.Branches.Remove(branch);
            await _context.SaveChangesAsync();
            return branch;
        }

        public async Task<IEnumerable<Branch>> GetAll()
        {
            var branches = await _context.Branches.ToListAsync();
            return branches;
        }

        public async Task<IEnumerable<Branch>> GetAllBrancheshByRestaurantId(string restaurantId)
        {
            var branches = await _context.Branches
                .Where(br => br.Restaurant.Id == restaurantId)
                .Include(br => br.Address)
                .ToListAsync();
            return branches;
        }

        public async Task<Branch> GetById(string id)
        {
            var branch = await _context.Branches.Include(a=>a.Address).FirstOrDefaultAsync(x => x.Id == id) ?? throw new NotFoundException($"BranchManager with ID {id} not found");
            return branch;
        }
        public async Task<Branch> GetByIdIcludeOrders(string id)
        {
            return await _context.Branches
                .Where(x => x.Id == id)
                .Include(o => o.Orders)
                .FirstOrDefaultAsync() ?? throw new NotFoundException();
        }

        public async Task<Branch> Update(Branch entity)
        {
            _context.Branches.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
