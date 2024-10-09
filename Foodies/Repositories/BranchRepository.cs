using Foodies.Common;
using Foodies.Data;

namespace Foodies.Repositories
{
    public class BranchRepository : IBaseRepository<Branch>
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
            var branch = await _context.Branches.FirstOrDefaultAsync(br => br.Id == id);
            _context.Branches.Remove(branch);
            await _context.SaveChangesAsync();
            return branch;
        }

        public async Task<IEnumerable<Branch>> GetAll()
        {
            var branches = await _context.Branches.ToListAsync();
            return branches;
        }

        public async Task<Branch> GetById(string id)
        {
            var branch = await _context.Branches.FirstOrDefaultAsync(x => x.Id == id);
            return branch;
        }

        public async Task<Branch> Update(Branch entity)
        {
            _context.Branches.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
