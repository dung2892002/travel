using Microsoft.AspNetCore.Http;
using Travel.Core.DTOs;
using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IServices
{
    public interface IReviewService
    {
        Task Create(Review review, List<IFormFile> files);
        Task<PagedResult<Review>> GetByHotel(Guid hoteId, int pageNumber);
        Task<PagedResult<Review>> GetByTour(Guid tourId, int pageNumber);
        Task<IEnumerable<Review>> GetByDestination(int destinationId);
        Task<bool> Delete(int id, Guid userId);
    }
}
