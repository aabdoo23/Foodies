using Foodies.Common;
using Foodies.Data;
using Foodies.Exceptions;
using Foodies.Interfaces.Repositories;

namespace Foodies.Repositories
{
    public class BranchManagerRepository : IBranchManagerRepository
    {
        private readonly FoodiesDbContext _context;
        public BranchManagerRepository(FoodiesDbContext context)
        {
            _context = context;
        }

        public async Task<BranchManager> Create(BranchManager entity)
        {
            _context.BranchManagers.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<BranchManager> Delete(string id)
        {
            var branchManager = await _context.BranchManagers.FirstOrDefaultAsync(x => x.Id == id) ?? throw new NotFoundException($"BranchManager with ID {id} not found");
            _context.BranchManagers.Remove(branchManager);
            await _context.SaveChangesAsync();
            return branchManager;
        }

        public async Task<IEnumerable<BranchManager>> GetAll()
        {
            var branchManagers = await _context.BranchManagers.ToListAsync();
            return branchManagers;
        }

        public async Task<BranchManager> GetById(string id)
        {
            var branchManager = await _context.BranchManagers.FirstOrDefaultAsync(x => x.Id == id) ?? throw new NotFoundException($"BranchManager with ID {id} not found");
            return branchManager;
        }

        public async Task<BranchManager> GetByIdWithBranchAndRestaurantIncluded(string id)
        {
            var branchManager = await _context.BranchManagers
                .Include(x => x.IdentityUser)
                .Include(x => x.Branch)
                    .ThenInclude(branch => branch.Restaurant) // Correctly includes Restaurant
                .Include(x => x.Branch)
                    .ThenInclude(branch => branch.Address) // Correctly includes Address
                .FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new NotFoundException($"BranchManager with ID {id} not found");
            return branchManager;

        }

        public async Task<BranchManager> Update(BranchManager entity)
        {
            _context.BranchManagers.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
