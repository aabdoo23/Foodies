using Foodies.Common;
using Foodies.Data;
using Foodies.Exceptions;
using Foodies.Interfaces.Repositories;

namespace Foodies.Repositories
{
    public class RatingRepository : IRatingRepository
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
            var rating = await _context.Ratings.FirstOrDefaultAsync(x => x.Id == id) ?? throw new NotFoundException($"Rating with {id} not found");
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
            return await _context.Ratings.FirstOrDefaultAsync(x => x.Id == id) ?? throw new NotFoundException($"Rating with {id} not found");
        }

        public async Task<Rating> Update(Rating entity)
        {
            _context.Ratings.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
