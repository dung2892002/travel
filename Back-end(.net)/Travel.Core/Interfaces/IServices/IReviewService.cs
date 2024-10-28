using Microsoft.AspNetCore.Http;
using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IServices
{
    public interface IReviewService
    {
        Task Create(Review review, List<IFormFile> files);
        Task<IEnumerable<Review>> GetByHotel(Guid hoteId);
        Task<IEnumerable<Review>> GetByTour(Guid tourId);
        Task<IEnumerable<Review>> GetByDestination(int destinationId);
        Task<bool> Delete(int id);
    }
}
