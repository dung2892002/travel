using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IRepositories
{
    public interface ITourRepository
    {
        Task Create(Tour tour);
        Task<Tour?> GetById(Guid id);
        Task<IEnumerable<Tour>> GetAll();
        Task<Tour?> GetDetail(Guid id);
        Task<IEnumerable<Tour>> GetByPartner(Guid partnerId);
        Task<IEnumerable<Tour>> GetByCity(int cityId);
        Task<IEnumerable<Tour>> GetByDepartureCity(int cityId);

        Task CreateTourDay(TourDay tourDay);
        Task CreateTourCity(TourCity tourCity);
        Task CreateActitity(Activity activity);
        Task CreateTourDetail(TourDetail tourDetail);
        Task CreateTourNotice(TourNotice tourNotice);
        Task CreateTourPrice(TourPrice tourPrice);
        Task CreateTourSchedule(TourSchedule tourSchedule);
        Task<IEnumerable<TourSchedule>> GetScheduleByTour(Guid tourId);
        Task<IEnumerable<TourSchedule>> GetScheduleAvailableByTour(Guid tourId);
        Task CreateTourRefund(Refund tourRefund);

        Task DeleteTourDay(TourDay tourDay);
        Task DeleteTourCity(TourCity tourCity);
        Task DeleteActitity(Activity activity);
        Task DeleteTourDetail(TourDetail tourDetail);
        Task DeleteTourNotice(TourNotice tourNotice);
        Task DeleteTourPrice(TourPrice tourPrice);
        Task DeleteTourRefund(Refund tourRefund);

        Task<IEnumerable<TourDay>> GetTourDaysByTour(Guid tourId);
        Task<IEnumerable<Activity>> GetActivitiesByTourDay(Guid tourDayId);
        Task<IEnumerable<TourCity>> GetTourCityByTour(Guid tourId);
        Task<IEnumerable<TourDetail>> GetTourDetailByTour(Guid tourId);
        Task<IEnumerable<TourNotice>> GetTourNoticeByTour(Guid tourId);
        Task<IEnumerable<TourPrice>> GetTourPriceByTour(Guid tourId);
        Task<IEnumerable<Refund>> GetTourRefundByTour(Guid tourId);

        Task<TourDetail?> GetTourDetailById(int id);
        Task<TourPrice?> GetTourPriceById(Guid id);
        Task<TourNotice?> GetTourNoticeById(int id);
        Task<Refund?> GetTourRefundById(int id);
        Task<TourCity?> GetTourCityById(int id);
        Task<Activity?> GetActivityById(Guid id);
        Task<TourDay?> GetTourDayById(Guid id);
    }
}
