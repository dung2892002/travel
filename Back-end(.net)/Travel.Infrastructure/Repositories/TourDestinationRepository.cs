using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel.Core.Entities;
using Travel.Core.Interfaces.IRepositories;
using Travel.Infrastructure.Data;

namespace Travel.Infrastructure.Repositories
{
    public class TourDestinationRepository(TravelDbContext dbContext) : ITourDestinationRepository, IRepository
    {
        private readonly TravelDbContext _dbContext = dbContext;
        public async Task Create(List<TourDestination> tourDestinations)
        {
            await _dbContext.AddRangeAsync(tourDestinations);
            await _dbContext.SaveChangesAsync();
        }
    }
}
