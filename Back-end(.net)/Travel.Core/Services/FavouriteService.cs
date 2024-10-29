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
    }
}
