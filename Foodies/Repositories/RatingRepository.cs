using Foodies.Common;
using Foodies.Data;

namespace Foodies.Repositories
{
    public class RatingRepository : IBaseRepository<Rating>
    {
        private readonly FoodiesDbContext _context;
        public RatingRepository(FoodiesDbContext context)
        {
            _context = context;
        }
        public async Task<Rating> Create(Rating entity)
        {
            await _context.Ratings.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Rating> Delete(string id)
        {
            var rating = await _context.Ratings.FirstOrDefaultAsync(x => x.Id == id);
            if (rating == null)
            {
                return null;
            }
            _context.Ratings.Remove(rating);
            await _context.SaveChangesAsync();
            return rating;
        }

        public async Task<IEnumerable<Rating>> GetAll()
        {
            return await _context.Ratings.ToListAsync();
        }

        public async Task<Rating> GetById(string id)
        {
            return await _context.Ratings.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Rating> Update(Rating entity)
        {
            _context.Ratings.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
