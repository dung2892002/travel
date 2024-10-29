using Microsoft.EntityFrameworkCore;
using Travel.Core.Entities;
using Travel.Core.Interfaces.IRepositories;
using Travel.Infrastructure.Data;

namespace Travel.Infrastructure.Repositories
{
    public class FavouriteRepository(TravelDbContext dbContext) : IFavouriteRepository, IRepository
    {
        private readonly TravelDbContext _dbContext = dbContext;

        public async Task Create(Favourite favourite)
        {
            await _dbContext.Favourite.AddAsync(favourite);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(Favourite favourite)
        {
            _dbContext.Favourite.Remove(favourite);
            await Task.CompletedTask;
        }

        public async Task<Favourite?> GetById(int id)
        {
            var favourite = await _dbContext.Favourite.SingleOrDefaultAsync(f => f.Id == id);
            return favourite;
        }

        public async Task<IEnumerable<Favourite>> GetByUser(Guid userId)
        {
            var favourites = await _dbContext.Favourite
                .Where(f => f.UserId == userId)
                .Include(f => f.Hotel).ThenInclude(h => h.Image)
                .Include(f => f.City)
                .Include(f => f.Tour).ThenInclude(t => t.Image)
                .Include(f => f.Destination).ThenInclude(d => d.Image)
                .ToListAsync();

            return favourites;
        }
    }
}
