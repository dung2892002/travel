using Microsoft.EntityFrameworkCore;
using Travel.Core.Entities;
using Travel.Core.Interfaces.IRepositories;
using Travel.Infrastructure.Data;

namespace Travel.Infrastructure.Repositories
{
    public class TourRepository(TravelDbContext dbContext) : ITourRepository, IRepository
    {
        private readonly TravelDbContext _dbContext = dbContext;
        public async Task Create(Tour tour)
        {
            await _dbContext.AddAsync(tour);
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateActitity(Activity activity)
        {
            await _dbContext.Activity.AddAsync(activity);
        }

        public async Task CreateTourCity(TourCity tourCity)
        {
            await _dbContext.TourCity.AddAsync(tourCity);
        }

        public async Task CreateTourDay(TourDay tourDay)
        {
            await _dbContext.TourDay.AddAsync(tourDay);
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateTourDetail(TourDetail tourDetail)
        {
            await _dbContext.TourDetail.AddAsync(tourDetail);
        }

        public async Task CreateTourNotice(TourNotice tourNotice)
        {
            await _dbContext.TourNotice.AddAsync(tourNotice);
        }

        public async Task CreateTourPrice(TourPrice tourPrice)
        {
            await _dbContext.TourPrice.AddAsync(tourPrice);
        }

        public async Task CreateTourRefund(Refund tourRefund)
        {
            await _dbContext.Refund.AddAsync(tourRefund);
        }

        public async Task CreateTourSchedule(TourSchedule tourSchedule)
        {
            await _dbContext.TourSchedule.AddAsync(tourSchedule);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteActitity(Activity activity)
        {
            _dbContext.Activity.Remove(activity);
            await Task.CompletedTask;
        }

        public async Task DeleteTourCity(TourCity tourCity)
        {
            _dbContext.TourCity.Remove(tourCity);
            await Task.CompletedTask;
        }

        public async Task DeleteTourDay(TourDay tourDay)
        {
            _dbContext.TourDay.Remove(tourDay);
            await Task.CompletedTask;
        }

        public async Task DeleteTourDetail(TourDetail tourDetail)
        {
            _dbContext.TourDetail.Remove(tourDetail);
            await Task.CompletedTask;
        }

        public async Task DeleteTourNotice(TourNotice tourNotice)
        {
            _dbContext.TourNotice.Remove(tourNotice);
            await Task.CompletedTask;
        }

        public async Task DeleteTourPrice(TourPrice tourPrice)
        {
            _dbContext.TourPrice.Remove(tourPrice);
            await Task.CompletedTask;
        }

        public async Task DeleteTourRefund(Refund tourRefund)
        {
            _dbContext.Refund.Remove(tourRefund);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Tour>> GetAll()
        {
            return await _dbContext.Tour.ToListAsync();
        }

        public async Task<IEnumerable<Tour>> GetByCity(int cityId)
        {
            return await _dbContext.Tour.Where(t => t.TourCity.Any(tc => tc.CityId == cityId)).ToListAsync(); 
        }

        public async Task<IEnumerable<Tour>> GetByDepartureCity(int cityId)
        {
            return await _dbContext.Tour.Where(t => t.DepartureCityId == cityId).ToListAsync();
        }

        public async Task<Tour?> GetById(Guid id)
        {
            return await _dbContext.Tour.SingleOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Tour?> GetDetail(Guid id)
        {
            return await _dbContext.Tour
                .AsSplitQuery()
                .Include(t => t.Image)
                .Include(t => t.TourDetail)
                .Include(t => t.DepartureCity)
                .Include(t => t.TourDay).ThenInclude(td => td.Activity)
                .Include(t => t.TourCity).ThenInclude(tc => tc.City)
                .Include(t => t.TourPrice)
                .Include(t => t.Refund)
                .Include(t => t.TourNotice)
                .SingleOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Tour>> GetByPartner(Guid partnerId)
        {
            return await _dbContext.Tour
                .Include(t => t.DepartureCity)
                    .ThenInclude(dc => dc.Province)
                .Where(t => t.UserId == partnerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<TourSchedule>> GetScheduleByTour(Guid tourId)
        {
            return await _dbContext.TourSchedule.Where(ts => ts.TourId == tourId).ToListAsync();
        }

        public async Task<IEnumerable<TourSchedule>> GetScheduleAvailableByTour(Guid tourId)
        {
            return await _dbContext.TourSchedule.Where(ts => ts.TourId == tourId && ts.DateStart.Date > DateTime.Now.Date ).ToListAsync();
        }

        public async Task<IEnumerable<TourDay>> GetTourDaysByTour(Guid tourId)
        {
            return await _dbContext.TourDay.Where(td => td.TourId == tourId).ToListAsync();
        }

        public async Task<IEnumerable<Activity>> GetActivitiesByTourDay(Guid tourDayId)
        {
            return await _dbContext.Activity.Where(a => a.TourDayId == tourDayId).ToListAsync();
        }

        public async Task<IEnumerable<TourCity>> GetTourCityByTour(Guid tourId)
        {
            return await _dbContext.TourCity.Where(tc => tc.TourId == tourId).ToListAsync();
        }

        public async Task<IEnumerable<TourDetail>> GetTourDetailByTour(Guid tourId)
        {
            return await _dbContext.TourDetail.Where(td => td.TourId==tourId).ToListAsync();
        }

        public async Task<IEnumerable<TourNotice>> GetTourNoticeByTour(Guid tourId)
        {
            return await _dbContext.TourNotice.Where(tn => tn.TourId == tourId).ToListAsync();
        }

        public async Task<IEnumerable<TourPrice>> GetTourPriceByTour(Guid tourId)
        {
            return await _dbContext.TourPrice.Where(p => p.TourId == tourId).ToListAsync();
        }

        public async Task<IEnumerable<Refund>> GetTourRefundByTour(Guid tourId)
        {
            return await _dbContext.Refund.Where(r => r.TourId == tourId).ToListAsync();
        }

        public async Task<TourDetail?> GetTourDetailById(int id)
        {
            return await _dbContext.TourDetail.SingleOrDefaultAsync(td => td.Id==id); 
        }

        public async Task<TourPrice?> GetTourPriceById(Guid id)
        {
            return await _dbContext.TourPrice.SingleOrDefaultAsync(_ => _.Id==id);
        }

        public async Task<TourNotice?> GetTourNoticeById(int id)
        {
            return await _dbContext.TourNotice.SingleOrDefaultAsync(_ => _.Id==id); 
        }

        public async Task<Refund?> GetTourRefundById(int id)
        {
            return await _dbContext.Refund.SingleOrDefaultAsync(r => r.Id==id);
        }

        public async Task<TourCity?> GetTourCityById(int id)
        {
            return await _dbContext.TourCity.SingleOrDefaultAsync(tc => tc.Id==id);
        }

        public async Task<Activity?> GetActivityById(Guid id)
        {
            return await _dbContext.Activity.SingleOrDefaultAsync(a => a.Id==id);
        }

        public async Task<TourDay?> GetTourDayById(Guid id)
        {
            return await _dbContext.TourDay.SingleOrDefaultAsync(td => td.Id==id);
        }
    }
}
