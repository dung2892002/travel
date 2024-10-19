using Microsoft.EntityFrameworkCore;
using Travel.Core.Entities;
using Travel.Core.Interfaces.IRepositories;
using Travel.Infrastructure.Data;

namespace Travel.Infrastructure.Repositories
{
    public class ImageRepository(TravelDbContext dbContext) : IImageRepository, IRepository
    {
        private readonly TravelDbContext _dbContext = dbContext;
        public async Task AddImage(Image image)
        {
            await _dbContext.Image.AddAsync(image);
        }

        public async Task<IEnumerable<Image>> GetByDestination(int destinationId)
        {
            return await _dbContext.Image.Where(i => i.DestinationId == destinationId).ToListAsync();
        }
    }
}
