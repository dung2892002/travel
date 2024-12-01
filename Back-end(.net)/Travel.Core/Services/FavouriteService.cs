using Travel.Core.Entities;
using Travel.Core.Interfaces;
using Travel.Core.Interfaces.IServices;

namespace Travel.Core.Services
{
    public class FavouriteService(IUnitOfWork unitOfWork) : IFavouriteService, IService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task Create(Favourite favourite)
        {
            await _unitOfWork.Favorites.Create(favourite);
        }

        public async Task<bool> Delete(int favouriteId)
        {
            var favourite = await _unitOfWork.Favorites.GetById(favouriteId);
            if (favourite == null)
            {
                return false;
            }
            await _unitOfWork.Favorites.Delete(favourite);
            var result = await _unitOfWork.CompleteAsync();
            return result > 0;
        }

        public async Task<int> GetQuantityByHotel(Guid hotelId)
        {
            var hotel = await _unitOfWork.Hotels.GetById(hotelId);
            if (hotel == null) throw new ArgumentException("hotel not exist");

            var quantity = await _unitOfWork.Favorites.GetQuantityByHotel(hotelId);
            return quantity;
        }

        public async Task<int> GetQuantityByTour(Guid tourId)
        {
            var tour = await _unitOfWork.Tours.GetById(tourId);
            if (tour == null) throw new ArgumentException("tour not exist");

            var quantity = await _unitOfWork.Favorites.GetQuantityByTour(tourId);
            return quantity;
        }

        public async Task<IEnumerable<Favourite>> GetByUser(Guid userId)
        {
            var user = await _unitOfWork.Users.GetUserById(userId);
            if (user == null)
            {
                throw new ArgumentException("user not exist");
            }    

            var favourites = await _unitOfWork.Favorites.GetByUser(userId);
            return favourites;
        }

        public async Task<Favourite?> GetUserFavouriteHotel(Guid userId, Guid hotelId)
        {
            var user = await _unitOfWork.Users.GetUserById(userId);
            if (user == null) throw new ArgumentException("user not exist");

            var hotel = await _unitOfWork.Hotels.GetById(hotelId);
            if (hotel == null) throw new ArgumentException("hotel not exist");

            return await _unitOfWork.Favorites.GetUserFavouriteHotel(userId, hotelId);
        }

        public async Task<Favourite?> GetUserFavouriteTour(Guid userId, Guid tourId)
        {
            var user = await _unitOfWork.Users.GetUserById(userId);
            if (user == null)  throw new ArgumentException("user not exist");

            var tour = await _unitOfWork.Tours.GetById(tourId);
            if (tour == null) throw new ArgumentException("tour not exist");

            return await _unitOfWork.Favorites.GetUserFavouriteTour(userId, tourId);
        }
    }
}
