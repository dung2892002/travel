using Microsoft.EntityFrameworkCore;
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
    }
}
