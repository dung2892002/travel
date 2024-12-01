using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IServices
{
    public interface IFavouriteService
    {
        Task Create(Favourite favourite);
        Task<IEnumerable<Favourite>> GetByUser(Guid userId);
        Task<bool> Delete(int favouriteId);
        Task<int> GetQuantityByHotel(Guid hotelId);
        Task<int> GetQuantityByTour(Guid tourId);
        Task<Favourite?> GetUserFavouriteHotel(Guid userId, Guid hotelId);
        Task<Favourite?> GetUserFavouriteTour(Guid userId, Guid tourId);

    }
}
