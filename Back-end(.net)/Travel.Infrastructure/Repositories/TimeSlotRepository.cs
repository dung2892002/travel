using Travel.Core.Entities;
using Travel.Core.Interfaces.IRepositories;
using Travel.Infrastructure.Data;

namespace Travel.Infrastructure.Repositories
{
    public class TimeSlotRepository(TravelDbContext dbContext) : ITimeSlotRepository, IRepository
    {
        private readonly TravelDbContext _dbContext = dbContext;
        public async Task Create(TimeSlot timeSlot)
        {
            await _dbContext.AddAsync(timeSlot);
            await _dbContext.SaveChangesAsync();
        }
    }
}
