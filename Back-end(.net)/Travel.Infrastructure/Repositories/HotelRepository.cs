using Microsoft.EntityFrameworkCore;
using Travel.Core.DTOs;
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
            var hotels = await _dbContext.Hotel
                .Include(x => x.Image)
                .Include(h => h.City)
                    .ThenInclude(c => c.Province)
                .Include(h => h.HotelFacility)
                    .ThenInclude(hf => hf.Facility)
                .ToListAsync();

            return hotels;


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
            var hotels = await _dbContext.Hotel
                .Include(x => x.Image)
                .Include(h => h.City)
                    .ThenInclude(c => c.Province)
                .Include(h => h.HotelFacility)
                    .ThenInclude(hf => hf.Facility)
                .Include(h => h.Room)
                .Where(h => h.UserId == partnerId)
                .ToListAsync();

            return hotels;
        }

        public async Task<PagedResult<Hotel>> SearchHotel(SearchHotelRequest request)
        {
            var query = _dbContext.Hotel
                .Include(h => h.Image)
                .Include(h => h.City)
                    .ThenInclude(c => c.Province)
                .Include(h => h.HotelFacility)
                    .ThenInclude(hf => hf.Facility)
                .Include(h => h.Room)
                    .ThenInclude(r => r.BookingRoom)
                .Include(h => h.Review)
                .AsQueryable();

            if (request.CityId.HasValue)
            {
                query = query.Where(h => h.CityId == request.CityId);
            }
            else if (request.ProvinceId.HasValue)
            {
                query = query.Where(h => h.City.ProvinceId == request.ProvinceId);
            }

            query = query.Where(h => h.Room.Any(r =>
                                               r.Quantity - r.BookingRoom
                                                           .Where(b => b.Status != 2 && ((b.CheckInDate < request.CheckOut && b.CheckOutDate > request.CheckIn)))
                                                           .Sum(b => b.Quantity)
                                               >= request.QuantityRoom));

            query = query.Where(h => h.Room.Any(r =>
                                                r.MaxAdultPeople >= (int)(request.QuantityAdultPeople / request.QuantityRoom)
                                                && r.MaxChildrenPeople >= (int)(request.QuantityChildrenPeople / request.QuantityRoom)));
            if (request.MinPrice.HasValue)
            {
                query = query.Where(h => h.Room.Any(r => r.Price >= request.MinPrice));
            }
            if (request.MaxPrice.HasValue)
            {
                query = query.Where(h => h.Room.Any(r => r.Price <= request.MaxPrice));
            }
            if (request.Types != null)
            {
                query = query.Where(h => request.Types.Contains(h.Type));
            }
            if (request.Ratings != null)
            {
                query = query.Where(h => request.Ratings.Contains(h.Rating));
            }
            if (request.HotelFacilities != null)
            {
                foreach (var facilityId in request.HotelFacilities)
                {
                    query = query.Where(h => h.HotelFacility.Any(hf => hf.FacilityId == facilityId));
                }
            }
            if (request.GuestRatings != null)
            {
                var minGuestRating = request.GuestRatings.Min();
                query = query.Where(h => h.Review.Count == 0 || h.Review.Average(r => r.Point) >= minGuestRating);
            }

            var totalCount = await query.CountAsync();

            var hotels = await query
                        .Select(h => new Hotel
                        {
                            Id = h.Id,
                            Name = h.Name,
                            CityId = h.CityId,
                            Rating = h.Rating,
                            Description = h.Description,
                            Type = h.Type,
                            City = h.City,
                            Image = h.Image,
                            HotelFacility = h.HotelFacility,
                            Review = h.Review,
                            Room = h.Room
                                    .Where(r =>
                                        r.Quantity - r.BookingRoom
                                            .Where(b => b.Status != 2 && (b.CheckInDate < request.CheckOut && b.CheckOutDate > request.CheckIn))
                                            .Sum(b => b.Quantity) >= request.QuantityRoom &&
                                        r.MaxAdultPeople >= (int)(request.QuantityAdultPeople / request.QuantityRoom) &&
                                        r.MaxChildrenPeople >= (int)(request.QuantityChildrenPeople / request.QuantityRoom) &&
                                        (!request.MinPrice.HasValue || r.Price >= request.MinPrice) &&
                                        (!request.MaxPrice.HasValue || r.Price <= request.MaxPrice)
                                    )
                                    .OrderBy(r => r.Price)
                                    .ToList() 
                        })
                        .Skip((request.PageNumber - 1) * 10)
                        .Take(10)
                        .ToListAsync();

            return new PagedResult<Hotel>
            {
                Items = hotels,
                TotalItems = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / 10.0)
            };  
        }

    }
}
