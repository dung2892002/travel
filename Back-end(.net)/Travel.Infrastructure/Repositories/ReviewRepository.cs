using Microsoft.EntityFrameworkCore;
using Travel.Core.DTOs;
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

        public async Task<PagedResult<Review>> GetByHotel(Guid hoteId, int pageNumber)
        {
            var query = _dbcontext.Review
                        .Where(r => r.HotelId == hoteId)
                        .OrderByDescending(r => r.CreatedAt);

            var averagePoint = await query.AverageAsync(r => r.Point);
            var quantityFantastic = await query.Where(r => r.Point == 10).CountAsync();
            var quantityVerygood = await query.Where(r => r.Point == 9).CountAsync();
            var quantitySatisfying = await query.Where(r => r.Point == 7 || r.Point == 8).CountAsync(); 
            var quantityAverage = await query.Where(r => r.Point == 5 || r.Point == 6).CountAsync();
            var quantityPoor = await query.Where(r => r.Point < 5).CountAsync();
            var totalCount = await query.CountAsync();

            var overall = new OverallReview
            {
                Average = Math.Round(averagePoint, 1),
                QuantityFantastic = quantityFantastic,
                QuantityVeryGood = quantityVerygood,
                QuantitySatisfying = quantitySatisfying,
                QuantityAverage = quantityAverage,
                QuantityPoor = quantityPoor,
            };
            var reviews = await query
                                .Skip((pageNumber - 1) * 10)
                                .Take(10)
                                .Include(r => r.Image)
                                .Include(r => r.User)
                                .ToListAsync();
            return new PagedResult<Review>
            {
                Items = reviews,
                TotalItems = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / 10.0),
                OverallReview = overall,
            };
        }

        public async Task<Review?> GetById(int id)
        {
            var review = await _dbcontext.Review
                .Include(r => r.Image)
                .Include(r => r.User)
                .SingleOrDefaultAsync(r => r.Id == id);
            return review;
        }

        public async Task<PagedResult<Review>> GetByTour(Guid tourId, int pageNumber)
        {
            var query = _dbcontext.Review
                .Include(r => r.Image)
                .Include(r => r.User)
                .Where(r => r.TourId == tourId)
                .OrderByDescending(r => r.CreatedAt);

            var averagePoint = await query.AverageAsync(r => r.Point);
            var quantityFantastic = await query.Where(r => r.Point == 10).CountAsync();
            var quantityVerygood = await query.Where(r => r.Point == 9).CountAsync();
            var quantitySatisfying = await query.Where(r => r.Point == 7 || r.Point == 8).CountAsync();
            var quantityAverage = await query.Where(r => r.Point == 5 || r.Point == 6).CountAsync();
            var quantityPoor = await query.Where(r => r.Point < 5).CountAsync();
            var totalCount = await query.CountAsync();

            var overall = new OverallReview
            {
                Average = Math.Round(averagePoint, 1),
                QuantityFantastic = quantityFantastic,
                QuantityVeryGood = quantityVerygood,
                QuantitySatisfying = quantitySatisfying,
                QuantityAverage = quantityAverage,
                QuantityPoor = quantityPoor,
            };
            var reviews = await query
                                .Skip((pageNumber - 1) * 10)
                                .Take(10)
                                .ToListAsync();
            return new PagedResult<Review>
            {
                Items = reviews,
                TotalItems = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / 10.0),
                OverallReview = overall,
            };
        }
    }
}
