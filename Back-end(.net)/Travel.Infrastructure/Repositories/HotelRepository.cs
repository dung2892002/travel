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

        public async Task CreateHotelRefund(Refund refund)
        {
            await _dbContext.Refund.AddAsync(refund);
        }

        public async Task DeleteHotelRefund(Refund refund)
        {
            _dbContext.Refund.Remove(refund);
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
                .Include(h => h.Refund.Where(r => r.State == true))
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

        public async Task<PagedResult<SearchHotelResponse>> SearchHotel(SearchHotelRequest request)
        {
            var query = _dbContext.Hotel.AsQueryable();

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

            var totalItems = await query.CountAsync();

            var hotels = await query
                        .Skip((request.PageNumber - 1) * 10)
                        .Take(10)
                        .Include(h => h.Image)
                        .Include(h => h.City)
                            .ThenInclude(c => c.Province)
                        .Include(h => h.HotelFacility)
                            .ThenInclude(hf => hf.Facility)
                        .Select(h => new SearchHotelResponse
                        {
                            Id = h.Id,
                            Name = h.Name,
                            Rating = h.Rating,
                            Type = h.Type,
                            AverageReview = h.Review.Any() ? Math.Round(h.Review.Average(r => r.Point), 1) : 0,
                            QuantityReview = h.Review.Count,
                            MinPrice = h.Room
                                        .Where(r =>
                                            r.Quantity - r.BookingRoom
                                                .Where(b => b.Status != 2 && (b.CheckInDate < request.CheckOut && b.CheckOutDate > request.CheckIn))
                                                .Sum(b => b.Quantity) >= request.QuantityRoom &&
                                            r.MaxAdultPeople >= (int)(request.QuantityAdultPeople / request.QuantityRoom) &&
                                            r.MaxChildrenPeople >= (int)(request.QuantityChildrenPeople / request.QuantityRoom) &&
                                            (!request.MinPrice.HasValue || r.Price >= request.MinPrice) &&
                                            (!request.MaxPrice.HasValue || r.Price <= request.MaxPrice)
                                        )
                                        .Min(r => r.Price),
                            CityName = h.City.Name,
                            ProvinceName = h.City.Province.Name,
                            HotelFacility = h.HotelFacility,
                            Image = h.Image
                        })
                        .ToListAsync();

            return new PagedResult<SearchHotelResponse>
            {
                Items = hotels,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / 10.0)
            };
        }

        public async Task<IEnumerable<Refund>> GetHotelRefundByHotel(Guid hotelId)
        {
            return await _dbContext.Refund.Where(r => r.HotelId == hotelId).ToListAsync();
        }

        public async Task<Refund?> GetHotelRefundById(int id)
        {
            return await _dbContext.Refund.SingleOrDefaultAsync(r => r.Id == id);
        }
    }
}
