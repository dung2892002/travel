using Microsoft.EntityFrameworkCore;
using Travel.Core.Entities;
using Travel.Core.Interfaces.IRepositories;
using Travel.Infrastructure.Data;

namespace Travel.Infrastructure.Repositories
{
    public class ReviewRepository(TravelDbContext dbContext) : IReviewRepository, IRepository
    {
        private readonly TravelDbContext _dbcontext = dbContext;
        public async Task Create(Review review)
        {
            await _dbcontext.Review.AddAsync(review);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task Delete(Review review)
        {
            _dbcontext.Review.Remove(review);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Review>> GetByDestination(int destinationId)
        {
            var reviews = await _dbcontext.Review
                .Include(r => r.Image)
                .Include(r => r.User)
                .Where(r => r.DestinationId == destinationId).ToListAsync();
            return reviews;
        }

        public async Task<IEnumerable<Review>> GetByHotel(Guid hoteId)
        {
            var reviews = await _dbcontext.Review
                .Include(r => r.Image)
                .Include(r => r.User)
                .Where(r => r.HotelId == hoteId).ToListAsync();
            return reviews;
        }

        public async Task<Review?> GetById(int id)
        {
            var review = await _dbcontext.Review
                .Include(r => r.Image)
                .Include(r => r.User)
                .SingleOrDefaultAsync(r => r.Id == id);
            return review;
        }

        public async Task<IEnumerable<Review>> GetByTour(Guid tourId)
        {
            var reviews = await _dbcontext.Review
                .Include(r => r.Image)
                .Include(r => r.User)
                .Where(r => r.TourId == tourId).ToListAsync();
            return reviews;
        }
    }
}
