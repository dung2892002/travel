using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IRepositories
{
    public interface IDiscountRepository
    {
        Task Create(Discount discount);
        Task<Discount?> GetById(Guid? id);
        Task CreateDiscountHotel(DiscountHotel discountHotel);
        Task CreateDiscountTour(DiscountTour discountTour);
        Task<IEnumerable<Discount>> GetByHotel(Guid hotelId, DateTime date, decimal price);
        Task<IEnumerable<Discount>> GetByTour(Guid tourId, DateTime date, decimal price);
        Task<IEnumerable<Discount>> GetByUser(Guid userId);
    }
}
