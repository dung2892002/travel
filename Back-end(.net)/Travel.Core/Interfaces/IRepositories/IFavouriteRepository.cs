using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IRepositories
{
    public interface IFavouriteRepository
    {
        Task Create(Favourite favourite);
        Task<IEnumerable<Favourite>> GetByUser(Guid userId);
        Task Delete(Favourite favourite);
        Task<Favourite?> GetById(int id);
        Task<int> GetQuantityByHotel(Guid hotelId);
        Task<int> GetQuantityByTour(Guid tourId);
        Task<Favourite?> GetUserFavouriteHotel(Guid userId, Guid hotelId);
        Task<Favourite?> GetUserFavouriteTour(Guid userId, Guid tourId);
    }
}
