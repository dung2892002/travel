using Microsoft.EntityFrameworkCore;
using Travel.Core.Entities;
using Travel.Core.Interfaces.IRepositories;
using Travel.Infrastructure.Data;

namespace Travel.Infrastructure.Repositories
{
    public class HotelRepository(TravelDbContext dbContext) : IHotelRepository, IRepository
    {
        private readonly TravelDbContext _dbContext = dbContext;
        public async Task CreateHotel(Hotel hotel)
        {
            await _dbContext.Hotel.AddAsync(hotel);
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateHotelDestination(HotelDestination hotelDestination)
        {
            await _dbContext.HotelDestination.AddAsync(hotelDestination);
        }
        public async Task DeleteHotelDestination(HotelDestination hotelDestination)
        {
            _dbContext.HotelDestination.Remove(hotelDestination);
            await Task.CompletedTask;
        }
        public async Task CreateHotelFacility(HotelFacility hotelFacility)
        {
            await _dbContext.HotelFacility.AddAsync(hotelFacility);
        }
        public async Task DeleteHotelFacility(HotelFacility hotelFacility)
        {
            _dbContext.HotelFacility.Remove(hotelFacility);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Hotel>> GetAllHotel()
        {
            return await _dbContext.Hotel.Include(x => x.Image).Select(h => new Hotel
            {
                Id = h.Id,
                Name = h.Name,
                Description = h.Description,
                Rating = h.Rating,
                Email = h.Email,
                PhoneNumber = h.PhoneNumber,
                CheckInTime = h.CheckInTime,
                CheckOutTime = h.CheckOutTime
            }).ToListAsync();


        }

        public async Task<IEnumerable<Hotel>> GetByDestination(int destinationId)
        {
            var hotels = await _dbContext.Hotel
                .Include(h => h.Image)
                .Include(h => h.City)
                    .ThenInclude(c => c.Province)
                .Include(h => h.HotelDestination)
                    .ThenInclude(hd => hd.Destination)
                .Include(h => h.HotelFacility)
                    .ThenInclude(hf => hf.Facility)
                .Where(h => h.HotelDestination.Any(hd => hd.DestinationId == destinationId))
                .ToListAsync();

            return hotels;
        }

        public async Task<Hotel?> GetById(Guid id)
        {
            var hotel = await _dbContext.Hotel
                .Include(x => x.Image)
                .Include(h => h.City)
                    .ThenInclude(c => c.Province)
                .Include(h => h.HotelDestination)
                    .ThenInclude(hd => hd.Destination)
                .Include(h => h.HotelFacility)
                    .ThenInclude(hf => hf.Facility)
                .SingleOrDefaultAsync(h => h.Id == id);
            return hotel;
        }

        public async Task<IEnumerable<Hotel>> GetByPartner(Guid partnerId)
        {
            return await _dbContext.Hotel
                .Include(x => x.Image)
                .Include(h => h.City)
                    .ThenInclude(c => c.Province)
                .Include(h => h.HotelFacility)
                    .ThenInclude(hf => hf.Facility)
                .Where(h => h.UserId == partnerId)
                .ToListAsync();
        }
    }
}
