using Microsoft.AspNetCore.Http;
using Travel.Core.DTOs;
using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IRepositories
{
    public interface IReviewRepository
    {
        Task Create(Review review);
        Task<PagedResult<Review>> GetByHotel(Guid hoteId, int pageNumber);
        Task<PagedResult<Review>> GetByTour(Guid tourId, int pageNumber);
        Task<IEnumerable<Review>> GetByDestination(int destinationId);
        Task<Review?> GetById(int id);
        Task Delete(Review review);
    }
}
