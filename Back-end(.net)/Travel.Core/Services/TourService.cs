using Microsoft.AspNetCore.Http;
using Travel.Core.DTOs;
using Travel.Core.Entities;
using Travel.Core.Interfaces;
using Travel.Core.Interfaces.IServices;

namespace Travel.Core.Services
{
    public class TourService(IUnitOfWork unitOfWork, IFirebaseStorageService firebaseStorageService) : ITourService, IService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;  
        private readonly IFirebaseStorageService _firebaseStorageService = firebaseStorageService;
        public async Task Create(Tour tour)
        {
            await _unitOfWork.BeginTransaction();
            try
            {
                tour.Id = Guid.NewGuid();
                await _unitOfWork.Tours.Create(tour);

                await _unitOfWork.CommitTransaction();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransaction();
                throw new ApplicationException("Error creating tour", ex);
            }
        }

        public async Task CreateSchedule(TourSchedule schedule)
        {
            var tour = await _unitOfWork.Tours.GetById(schedule.TourId);
            if (tour == null)
            {
                throw new ArgumentException("tour not exist");
            }

            schedule.Id = Guid.NewGuid();
            await _unitOfWork.Tours.CreateTourSchedule(schedule);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<Tour>> GetAll()
        {
            return await _unitOfWork.Tours.GetAll();
        }

        public async Task<IEnumerable<Tour>> GetByCity(int cityId)
        {
            return await _unitOfWork.Tours.GetByCity(cityId);
        }

        public async Task<IEnumerable<Tour>> GetByDepartureCity(int cityId)
        {
            return await _unitOfWork.Tours.GetByDepartureCity(cityId);
        }

        public async Task<Tour?> GetById(Guid id)
        {
            return await _unitOfWork.Tours.GetById(id);
        }

        public async Task<Tour?> GetDetail(Guid id)
        {
            return await _unitOfWork.Tours.GetDetail(id);
        }

        public async Task<IEnumerable<Tour>> GetByPartner(Guid partnerId)
        {
            return await _unitOfWork.Tours.GetByPartner(partnerId);
        }

        public async Task<bool> UploadImagesAsync(List<IFormFile> files, Guid tourId)
        {
            if (files == null || files.Count == 0)
                throw new ArgumentException("No files were provided.");
            var filePaths = new List<string>();

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var path = DateTime.Now.ToString("dd-MM-yyyy") + "_" + Guid.NewGuid().ToString() + "_" + file.FileName;
                    using Stream stream = file.OpenReadStream();
                    var result = await _firebaseStorageService.UploadFile(stream, path);
                    if (!result) throw new Exception("Upload image error");

                    var image = new Image
                    {
                        Path = path,
                        TourId = tourId,
                    };

                    await _unitOfWork.Images.AddImage(image);
                }
            }
            await _unitOfWork.CompleteAsync();
            return true;
        }

        private static async Task HandleTourDays(Tour tour, IUnitOfWork _unitOfWork)
        {

            foreach (var tourDay in tour.TourDay)
            {
                tourDay.Id = Guid.NewGuid();
                tourDay.TourId = tour.Id;
                await _unitOfWork.Tours.CreateTourDay(tourDay);

                foreach (var activity in tourDay.Activity)
                {
                    activity.Id = Guid.NewGuid();
                    activity.TourDayId = tourDay.Id;
                    await _unitOfWork.Tours.CreateActitity(activity);
                }
            }
        }
        
        private static async Task HandleTourCities(Tour tour, IUnitOfWork _unitOfWork)
        {
            foreach (var tourCity in tour.TourCity)
            {
                tourCity.TourId = tour.Id;
                tourCity.City = null;
                await _unitOfWork.Tours.CreateTourCity(tourCity);
            }
        }

        private static async Task HandleTourDetails(Tour tour, IUnitOfWork _unitOfWork)
        {
            foreach (var tourDetail in tour.TourDetail)
            {
                tourDetail.TourId = tour.Id;
                await _unitOfWork.Tours.CreateTourDetail(tourDetail);
            }
        }

        private static async Task HandleTourNotices(Tour tour, IUnitOfWork _unitOfWork)
        {            
            foreach (var tourNotice in tour.TourNotice)
            {
                tourNotice.TourId = tour.Id;
                await _unitOfWork.Tours.CreateTourNotice(tourNotice);
            }
        }

        private static async Task HandleTourPrices(Tour tour, IUnitOfWork _unitOfWork)
        {
            foreach (var tourPrice in tour.TourPrice)
            {
                tourPrice.TourId = tour.Id;
                tourPrice.State = true;
                await _unitOfWork.Tours.CreateTourPrice(tourPrice);
            }
        }

        private static async Task HandleTourRefunds(Tour tour, IUnitOfWork _unitOfWork)
        {
            foreach (var refund in tour.Refund) {
                refund.TourId = tour.Id;
                refund.Tour = null; // Đặt null nếu không cần xử lý thêm Tour
                refund.Hotel = null; // Đặt null nếu không cần xử lý thêm Hotel
                await _unitOfWork.Tours.CreateTourRefund(refund);
            }
        }

        public async Task<IEnumerable<TourSchedule>> GetScheduleByTour(Guid tourId)
        {
            var tour = await _unitOfWork.Tours.GetById(tourId);
            if (tour == null)
            {
                throw new ArgumentException("tour not exist");
            }

            var schedules = await _unitOfWork.Tours.GetScheduleByTour(tourId);
            return schedules;
        }

        public async Task<IEnumerable<TourSchedule>> GetScheduleAvailableByTour(Guid tourId)
        {
            var tour = await _unitOfWork.Tours.GetById(tourId);
            if (tour == null)
            {
                throw new ArgumentException("tour not exist");
            }

            var schedules = await _unitOfWork.Tours.GetScheduleAvailableByTour(tourId);
            return schedules;
        }

        public async Task Update(Tour tour, Guid id)
        {
            await _unitOfWork.BeginTransaction();
            try
            {
                var existingTour = await _unitOfWork.Tours.GetDetail(id) ?? throw new ArgumentException("Tour not exist");
                tour.Id = id;
                existingTour.Name = tour.Name;
                existingTour.NumberOfDay = tour.NumberOfDay;
                existingTour.NumberOfNight = tour.NumberOfNight;
                existingTour.Rating = tour.Rating;
                existingTour.DepartureCityId = tour.DepartureCityId;
                existingTour.Transport = tour.Transport;
                existingTour.Description = tour.Description;

                await HandleTourDaysForUpdate(tour);
                await HandleTourCitiesForUpdate(tour);
                await HandleTourDetailsForUpdate(tour);
                await HandleTourNoticesForUpdate(tour);
                await HandleTourPricesForUpdate(tour);
                await HandleTourRefundsForUpdate(tour);


                var result = await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransaction();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransaction();
                Console.WriteLine(ex.ToString());
                throw new ApplicationException("Error updating tour", ex);
            }
        }
        private async Task HandleTourDaysForUpdate(Tour tour)
        {
            var existingTourDays = await _unitOfWork.Tours.GetTourDaysByTour(tour.Id);

            var tourDaysToRemove = existingTourDays
                .Where(existing => !tour.TourDay.Any(newDay => newDay.Id == existing.Id));
            foreach (var tourDay in tourDaysToRemove)
            {
                await _unitOfWork.Tours.DeleteTourDay(tourDay);
            }

            foreach (var tourDay in tour.TourDay)
            {
                if (tourDay.Id == Guid.Empty)
                {
                    tourDay.Id = Guid.NewGuid();
                    tourDay.TourId = tour.Id;
                    await _unitOfWork.Tours.CreateTourDay(tourDay);
                }
                else
                {
                    var existingDay = await _unitOfWork.Tours.GetTourDayById(tourDay.Id);
                    existingDay.DayNumber = tourDay.DayNumber;
                    existingDay.Description = tourDay.Description;
                }

                await HandleActivitiesForUpdate(tourDay);
            }
        }

        private async Task HandleActivitiesForUpdate(TourDay tourDay)
        {
            var existingActivities = await _unitOfWork.Tours.GetActivitiesByTourDay(tourDay.Id);

            var activitiesToRemove = existingActivities
                .Where(existing => !tourDay.Activity.Any(newActivity => newActivity.Id == existing.Id));
            foreach (var activity in activitiesToRemove)
            {
                await _unitOfWork.Tours.DeleteActitity(activity);
            }
            
            foreach (var activity in tourDay.Activity)
            {
                if (activity.Id == Guid.Empty)
                {
                    activity.Id = Guid.NewGuid();
                    activity.TourDayId = tourDay.Id;
                    await _unitOfWork.Tours.CreateActitity(activity);
                }
                else
                {
                    var existingActivity = await _unitOfWork.Tours.GetActivityById(activity.Id);
                    if (existingActivity != null)
                    {
                        existingActivity.Description = activity.Description;
                    }
                    else
                    {
                        throw new InvalidOperationException($"Activity with ID {activity.Id} and description: {activity.Description} not found.");
                    }
                    //var result = await _unitOfWork.CompleteAsync();
                }
            }
        }

        private async Task HandleTourCitiesForUpdate(Tour tour)
        {
            var existingTourCities = await _unitOfWork.Tours.GetTourCityByTour(tour.Id);

            var tourCitiesToRemove = existingTourCities
                .Where(existing => !tour.TourCity.Any(newTourCity => newTourCity.Id == existing.Id));

            foreach (var tourCity in tourCitiesToRemove)
            {
                await _unitOfWork.Tours.DeleteTourCity(tourCity);
            }

            foreach (var tourCity in tour.TourCity)
            {
                if (tourCity.Id == 0)
                {
                    tourCity.TourId = tour.Id;
                    await _unitOfWork.Tours.CreateTourCity(tourCity);
                }
                else
                {
                    var existingCity = await _unitOfWork.Tours.GetTourCityById(tourCity.Id);
                    existingCity.CityId = tourCity.CityId;
                    existingCity.VisitOrder = tourCity.VisitOrder;

                    //var result = await _unitOfWork.CompleteAsync();
                }
            }
        }

        private async Task HandleTourNoticesForUpdate(Tour tour)
        {
            var existingTourNotices = await _unitOfWork.Tours.GetTourNoticeByTour(tour.Id);

            var tourNoticesToRemove = existingTourNotices
                .Where(existing => !tour.TourNotice.Any(newTourNotice => newTourNotice.Id == existing.Id));

            foreach (var tourNotice in tourNoticesToRemove)
            {
                await _unitOfWork.Tours.DeleteTourNotice(tourNotice);
            }

            foreach (var tourNotice in tour.TourNotice)
            {
                if (tourNotice.Id == 0)
                {
                    tourNotice.TourId = tour.Id;
                    await _unitOfWork.Tours.CreateTourNotice(tourNotice);
                }
                else
                {
                    var existingNotice = await _unitOfWork.Tours.GetTourNoticeById(tourNotice.Id);
                    existingNotice.Description = tourNotice.Description;
                    //var result = await _unitOfWork.CompleteAsync();
                }
            }
        }

        private async Task HandleTourDetailsForUpdate(Tour tour)
        {
            var existingTourDetails = await _unitOfWork.Tours.GetTourDetailByTour(tour.Id);

            var tourDetailsToRemove = existingTourDetails
                .Where(existing => !tour.TourDetail.Any(newTourDetail => newTourDetail.Id == existing.Id));

            foreach (var tourDetail in tourDetailsToRemove)
            {
                await _unitOfWork.Tours.DeleteTourDetail(tourDetail);
            }

            foreach (var tourDetail in tour.TourDetail)
            {
                if (tourDetail.Id == 0)
                {
                    tourDetail.TourId = tour.Id;
                    await _unitOfWork.Tours.CreateTourDetail(tourDetail);
                }
                else
                {
                    var detail = await _unitOfWork.Tours.GetTourDetailById(tourDetail.Id);
                    detail.Description = tourDetail.Description;

                    //var result = await _unitOfWork.CompleteAsync();
                }
            }
        }

        private async Task HandleTourPricesForUpdate(Tour tour)
        {
            var existingTourPrices = await _unitOfWork.Tours.GetTourPriceByTour(tour.Id);

            var tourPricesToRemove = existingTourPrices
                .Where(existing => !tour.TourPrice.Any(newTourPrice => newTourPrice.Id == existing.Id));

            foreach (var tourPrice in tourPricesToRemove)
            {
                tourPrice.State = false;  
            }

            foreach (var tourPrice in tour.TourPrice)
            {
                if (tourPrice.Id == Guid.Empty)
                {
                    tourPrice.Id = Guid.NewGuid();
                    tourPrice.TourId = tour.Id;
                    tourPrice.State = true;
                    await _unitOfWork.Tours.CreateTourPrice(tourPrice);
                }
                else
                {
                    var existingPrice = await _unitOfWork.Tours.GetTourPriceById(tourPrice.Id);
                    existingPrice.AgeStart = tourPrice.AgeStart;
                    existingPrice.AgeEnd = tourPrice.AgeEnd;
                    existingPrice.Percent = tourPrice.Percent;

                    //var result = await _unitOfWork.CompleteAsync();
                }
            }
        }

        private async Task HandleTourRefundsForUpdate(Tour tour)
        {
            var existingRefunds = await _unitOfWork.Tours.GetTourRefundByTour(tour.Id);

            var refundsToRemove = existingRefunds
                .Where(existing => !tour.Refund.Any(newRefund => newRefund.Id == existing.Id));

            foreach (var refund in refundsToRemove)
            {
                refund.State = false;
            }

            foreach (var refund in tour.Refund)
            {
                if (refund.Id == 0)
                {
                    refund.TourId = tour.Id;
                    refund.State = true;
                    await _unitOfWork.Tours.CreateTourRefund(refund);
                }
                else
                {
                    var existingRefund = await _unitOfWork.Tours.GetTourRefundById(refund.Id);
                    existingRefund.DayBefore = refund.DayBefore;
                    existingRefund.RefundPercent = refund.RefundPercent;

                    //var result = await _unitOfWork.CompleteAsync();
                }
            }
        }

        public async Task<bool> UpdatePriceSchedule(TourSchedule schedule, Guid id)
        {
            var existingSchedule = await _unitOfWork.Tours.GetTourScheduleById(id) ?? throw new ArgumentException("schedule not exist");

            existingSchedule.Price = schedule.Price;

            var result = await _unitOfWork.CompleteAsync();

            return result > 0;

        }

        public async Task<PagedResult<SearchTourResponse>> SearchTour(SearchTourRequest request)
        {
            return await _unitOfWork.Tours.SearchTour(request);
        }

        public async Task<IEnumerable<TourSchedule>> SearchSchedule(SearchScheduleRequest request)
        {
            return await _unitOfWork.Tours.SearchSchedule(request);
        }

        public async Task<TourSchedule?> GetScheduleDetail(Guid id)
        {
            var schedule = await _unitOfWork.Tours.GetScheduleDetail(id) ?? throw new ArgumentException("schedule not exist"); ;

            return schedule;
        }
    }
}
