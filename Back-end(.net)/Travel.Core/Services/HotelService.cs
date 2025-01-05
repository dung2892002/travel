using Microsoft.AspNetCore.Http;
using Travel.Core.DTOs;
using Travel.Core.Entities;
using Travel.Core.Interfaces;
using Travel.Core.Interfaces.IServices;

namespace Travel.Core.Services
{
    public class HotelService(IUnitOfWork unitOfWork, IFirebaseStorageService firebaseStorageService) : IHotelService, IService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IFirebaseStorageService _firebaseStorageService = firebaseStorageService;
        
        public async Task CreateHotel(Hotel hotel)
        {
            await _unitOfWork.BeginTransaction();
            try
            {
                hotel.Id = Guid.NewGuid();
                await _unitOfWork.Hotels.CreateHotel(hotel);
                foreach (var hotelDestination in hotel.HotelDestination)
                {
                    hotelDestination.HotelId = hotel.Id;
                    hotelDestination.Destination = null;
                    await _unitOfWork.Hotels.CreateHotelDestination(hotelDestination);
                }

                foreach (var hotelFacility in hotel.HotelFacility)
                {
                    hotelFacility.HotelId = hotel.Id;
                    hotelFacility.Facility = null;
                    await _unitOfWork.Hotels.CreateHotelFacility(hotelFacility);
                }
                foreach (var refund in hotel.Refund)
                {
                    refund.HotelId = hotel.Id;
                    refund.Hotel = null;
                    refund.Tour = null;
                    refund.State = true;
                    await _unitOfWork.Hotels.CreateHotelRefund(refund);
                }
                await _unitOfWork.CommitTransaction();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransaction();
                throw new ApplicationException("Error creating hotel", ex);
            }
        }

        public async Task<IEnumerable<Hotel>> GetAll()
        {
            return await _unitOfWork.Hotels.GetAllHotel();
        }

        public async Task<IEnumerable<Hotel>> GetByDestination(int destiantionId)
        {
            return await _unitOfWork.Hotels.GetByDestination(destiantionId);
        }

        public async Task<Hotel?> GetById(Guid id)
        {
            var hotel = await _unitOfWork.Hotels.GetById(id);
            if (hotel == null)
            {
                throw new ArgumentException("Hotel not exist");
            }
            return hotel;
        }

        public async Task<IEnumerable<Hotel>> GetByPartner(Guid partnerId)
        {
            return await _unitOfWork.Hotels.GetByPartner(partnerId);
        }

        public async Task<PagedResult<SearchHotelResponse>> SearchHotel(SearchHotelRequest request)
        {
            return await _unitOfWork.Hotels.SearchHotel(request);
        }

        public async Task<bool> UpdateHotel(Guid id, Hotel hotel)
        {
            await _unitOfWork.BeginTransaction();
            try
            {
                var hotelExisting = await _unitOfWork.Hotels.GetById(id) ?? throw new ArgumentException("Hotel not exist");
                hotelExisting.Name = hotel.Name;
                hotelExisting.Description = hotel.Description;
                hotelExisting.Rating = hotel.Rating;
                hotelExisting.CheckInTime = hotel.CheckInTime;
                hotelExisting.CheckOutTime = hotel.CheckOutTime;
                hotelExisting.Email = hotel.Email;
                hotelExisting.PhoneNumber = hotel.PhoneNumber;
                hotelExisting.CityId = hotel.CityId;
                hotelExisting.Type = hotel.Type;
                hotel.Id = hotelExisting.Id;
                await UpdateHotelDestinations(hotelExisting, hotel.HotelDestination);
                await UpdateHotelFacilities(hotelExisting, hotel.HotelFacility);
                await UpdateHotelRefunds(hotel);

                var result = await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransaction();

                return true;
            }


            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransaction();
                throw new ApplicationException("Error update hotel", ex);
            }
        }

        public async Task<bool> UploadImagesAsync(List<IFormFile> files, Guid hotelId)
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
                        HotelId = hotelId,
                    };

                    await _unitOfWork.Images.AddImage(image);
                }
            }
            await _unitOfWork.CompleteAsync();
            return true;
        }

        private async Task UpdateHotelDestinations(Hotel existingHotel, ICollection<HotelDestination> newDestinations)
        {
            var hotelDestinationRemove = existingHotel.HotelDestination
                .Where(d => !newDestinations.Any(nd => nd.DestinationId == d.DestinationId))
                .ToList();
            foreach (var hotelDestination in hotelDestinationRemove)
            {
                await _unitOfWork.Hotels.DeleteHotelDestination(hotelDestination);
            }

            var existingIds = existingHotel.HotelDestination.Select(d => d.DestinationId).ToList();
            var hotelDestinationAdd = newDestinations.Where(nd => !existingIds.Contains(nd.DestinationId)).ToList();
            foreach (var hotelDestination in hotelDestinationAdd)
            {
                hotelDestination.HotelId = existingHotel.Id;
                await _unitOfWork.Hotels.CreateHotelDestination(hotelDestination);
            }
        }

        private async Task UpdateHotelRefunds(Hotel hotel)
        {
            var existingRefunds = await _unitOfWork.Hotels.GetHotelRefundByHotel(hotel.Id);

            var refundsToRemove = existingRefunds
                .Where(existing => !hotel.Refund.Any(newRefund => newRefund.Id == existing.Id));

            foreach (var refund in refundsToRemove)
            {
                refund.State = false;
            }

            foreach (var refund in hotel.Refund)
            {
                if (refund.Id == 0)
                {
                    refund.HotelId = hotel.Id;
                    refund.State = true;
                    await _unitOfWork.Hotels.CreateHotelRefund(refund);
                }
                else
                {
                    var existingRefund = await _unitOfWork.Hotels.GetHotelRefundById(refund.Id);
                    existingRefund.DayBefore = refund.DayBefore;
                    existingRefund.RefundPercent = refund.RefundPercent;

                    //var result = await _unitOfWork.CompleteAsync();
                }
            }
        }
        private async Task UpdateHotelFacilities(Hotel existingHotel, ICollection<HotelFacility> newFacilities)
        {
            var hotelFacilityRemove = existingHotel.HotelFacility
                .Where(f => !newFacilities.Any(nf => nf.FacilityId == f.FacilityId))
                .ToList();
            foreach (var hotelFacility in hotelFacilityRemove)
            {
                await _unitOfWork.Hotels.DeleteHotelFacility(hotelFacility);
            }

            var existingIds = existingHotel.HotelFacility.Select(f => f.FacilityId).ToList();
            var hotelFacilityAdd = newFacilities.Where(nf => !existingIds.Contains(nf.FacilityId)).ToList();
            foreach (var hotelFacility in hotelFacilityAdd)
            {
                hotelFacility.HotelId = existingHotel.Id;
                await _unitOfWork.Hotels.CreateHotelFacility(hotelFacility);
            }
        }

        private bool IsSameRefund(Refund r1, Refund r2)
        {
            return r1.DayBefore == r2.DayBefore &&
                   r1.RefundPercent == r2.RefundPercent;
        }
    }
}
