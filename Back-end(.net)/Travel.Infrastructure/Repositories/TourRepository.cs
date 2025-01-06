using Microsoft.EntityFrameworkCore;
using Travel.Core.DTOs;
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
                .Include(t => t.TourPrice.Where(tp => tp.State == true))
                .Include(t => t.Refund.Where(r => r.State == true))
                .Include(t => t.TourNotice)
                .SingleOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Tour>> GetByPartner(Guid partnerId)
        {
            return await _dbContext.Tour
                .Include(t => t.Image)
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

        public async Task<TourSchedule?> GetTourScheduleById(Guid id)
        {
            return await _dbContext.TourSchedule.SingleOrDefaultAsync(s => s.Id==id);
        }

        public async Task<PagedResult<SearchTourResponse>> SearchTour(SearchTourRequest request)
        {
            var query = _dbContext.Tour.AsQueryable();

            if (request.CityId.HasValue)
            {
                query = query.Where(t => t.TourCity.Any(tc => tc.CityId == request.CityId));
            }
            else if (request.ProvinceId.HasValue)
            {
                query = query.Where(t => t.TourCity.Any(tc => tc.City.ProvinceId ==request.ProvinceId));
            }

            if (request.DateStart.HasValue) query = query.Where(t => t.TourSchedule.Any(ts => ts.DateStart >= request.DateStart));

            if (request.MinPrice.HasValue) query = query.Where(t => t.TourSchedule.Any(ts => ts.Price >= request.MinPrice));

            if (request.MaxPrice.HasValue) query = query.Where(t => t.TourSchedule.Any(ts => ts.Price <= request.MaxPrice));

            if (request.Ratings != null) query = query.Where(t => request.Ratings.Contains(t.Rating));

            if (request.GuestRatings != null)
                query = query.Where(t => t.Review.Count == 0 || t.Review.Average(r => r.Point) >= request.GuestRatings.Min());

            var totalItems = await query.CountAsync();

            var tours = await query
                        .Skip((request.PageNumber - 1) * 10)
                        .Take(10)
                        .Include(t => t.Image)
                        .Include(t => t.DepartureCity)
                            .ThenInclude(dc => dc.Province)
                        .Select(t => new SearchTourResponse
                        {
                            Id = t.Id,
                            Name = t.Name,
                            Rating = t.Rating,
                            Transport = t.Transport,
                            NumberOfDay = t.NumberOfDay,
                            NumberOfNight = t.NumberOfNight,
                            AverageReview = t.Review.Count != 0 ? Math.Round(t.Review.Average(r => r.Point), 2) : 0,
                            QuantityReview = t.Review.Count,
                            Image = t.Image,
                            DepartureCity = t.DepartureCity,
                            MinPrice = t.TourSchedule
                                        .Where(
                                                s => (!request.MinPrice.HasValue || s.Price >= request.MinPrice) &&
                                                (!request.MaxPrice.HasValue || s.Price <= request.MaxPrice) &&
                                                (!request.DateStart.HasValue || s.DateStart >= request.DateStart)
                                        )
                                        .Min(ts => ts.Price),

                        })
                        .ToListAsync();
            return new PagedResult<SearchTourResponse>
            {
                Items = tours,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / 10.0)
            };
        }

        public async Task<IEnumerable<TourSchedule>> SearchSchedule(SearchScheduleRequest request)
        {
           var query = _dbContext.TourSchedule.AsQueryable();
            query = query.Where(s => s.TourId == request.TourId).Where(s => s.DateStart >= request.DateStart);

            if (request.MinPrice.HasValue) query = query.Where(s => s.Price >= request.MinPrice);
            if (request.MaxPrice.HasValue) query = query.Where(s => s.Price <= request.MaxPrice);

            var schedules = await query.OrderBy(s => s.DateStart).ToListAsync();

            return schedules;
        }

        public async Task<TourSchedule?> GetScheduleDetail(Guid id)
        {
            return await _dbContext.TourSchedule
                .SingleOrDefaultAsync(ts => ts.Id == id);
        }
    }
}
