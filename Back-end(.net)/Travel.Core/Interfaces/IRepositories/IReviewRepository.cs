using Microsoft.AspNetCore.Http;
using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IRepositories
{
    public interface IReviewRepository
    {
        Task Create(Review review);
        Task<IEnumerable<Review>> GetByHotel(Guid hoteId);
        Task<IEnumerable<Review>> GetByTour(Guid tourId);
        Task<IEnumerable<Review>> GetByDestination(int destinationId);
        Task<Review?> GetById(int id);
        Task Delete(Review review);
    }
}
