using Microsoft.EntityFrameworkCore;
using Travel.Core.DTOs;
using Travel.Core.Entities;
using Travel.Core.Interfaces.IRepositories;
using Travel.Infrastructure.Data;

namespace Travel.Infrastructure.Repositories
{
    public class RoomRepository(TravelDbContext dbContext) : IRoomRepository, IRepository
    {
        private readonly TravelDbContext _dbContext = dbContext;    
        public async Task CreateRoom(Room room)
        {
            await _dbContext.Room.AddAsync(room);
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateRoomFacility(RoomFacility roomFacility)
        {
            await _dbContext.RoomFacility.AddAsync(roomFacility);
        }

        public async Task DeleteRoomFacility(RoomFacility roomFacility)
        {
            _dbContext.RoomFacility.Remove(roomFacility);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Room>> GetByHotel(Guid hotelId)
        {
            var rooms = await _dbContext.Room
                .Include(r => r.Image)
                .Include(r => r.RoomFacility)
                    .ThenInclude(rf => rf.Facility)
                .Where(r => r.HotelId == hotelId)
                .ToListAsync();
            return rooms;
        }

        public async Task<Room?> GetById(Guid id)
        {
            return await _dbContext.Room.SingleOrDefaultAsync(r => r.Id == id);
        }

        public Task<Room?> GetDetail(Guid id)
        {
            return _dbContext.Room
                .Include(r => r.Image)
                .Include(r => r.RoomFacility)
                    .ThenInclude(rf => rf.Facility)
                 .SingleOrDefaultAsync(r => r.Id ==id);    
        }

        public async Task<IEnumerable<Room>> SearchRoom(SearchRoomRequest request)
        {
            var query = _dbContext.Room
                .Include(r => r.Image)
                .Include(r => r.RoomFacility)
                    .ThenInclude(rf => rf.Facility)
                .Include(r => r.BookingRoom)
                .AsSplitQuery(); 

            query = query.Where(r =>
                r.Quantity - r.BookingRoom
                                .Where(b => b.Status != 2
                                            && b.CheckInDate < request.CheckOut
                                            && b.CheckOutDate > request.CheckIn)
                                .Sum(b => b.Quantity)
                >= request.QuantityRoom);

            var maxAdultsPerRoom = request.QuantityRoom > 0 ? (int)(request.QuantityAdultPeople / request.QuantityRoom) : 0;
            var maxChildrenPerRoom = request.QuantityRoom > 0 ? (int)(request.QuantityChildrenPeople / request.QuantityRoom) : 0;

            query = query.Where(r => r.MaxAdultPeople >= maxAdultsPerRoom
                                    && r.MaxChildrenPeople >= maxChildrenPerRoom);

            if (request.MinPrice.HasValue)
            {
                query = query.Where(r => r.Price >= request.MinPrice);
            }
            if (request.MaxPrice.HasValue)
            {
                query = query.Where(r => r.Price <= request.MaxPrice);
            }

            query = query.Where(r => r.HotelId == request.HotelId);

            return await query.ToListAsync();
        }

    }
}
