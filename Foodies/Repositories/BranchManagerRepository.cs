using Foodies.Common;
using Foodies.Data;

namespace Foodies.Repositories
{
    public class BranchManagerRepository : IBaseRepository<BranchManager>
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
            var branchManager = await _context.BranchManagers.FirstOrDefaultAsync(x => x.Id == id);
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
            var branchManager = await _context.BranchManagers.FirstOrDefaultAsync(x => x.Id == id);
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
