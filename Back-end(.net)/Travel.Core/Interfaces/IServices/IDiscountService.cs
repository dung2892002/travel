using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IServices
{
    public interface IDiscountService
    {
        Task CreateDiscountHotel(Discount discount);
        Task CreateDiscountTour(Discount discount);
        Task AdminCreate(Discount discount);
        Task<Discount?> GetById(Guid id);
        Task<IEnumerable<Discount>> GetByHotel(Guid hotelId, decimal price);
        Task<IEnumerable<Discount>> GetByTour(Guid tourId, decimal price);
        Task<IEnumerable<Discount>> GetByUser(Guid userId);
    }
}
