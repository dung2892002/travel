using Travel.Core.Entities;
using Travel.Core.Interfaces;
using Travel.Core.Interfaces.IServices;

namespace Travel.Core.Services
{
    public class DiscountService(IUnitOfWork unitOfWork) : IDiscountService, IService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task AdminCreate(Discount discount)
        {
            discount.Id = Guid.NewGuid();
            await _unitOfWork.Discounts.Create(discount);
        }

        public async Task CreateDiscountHotel(Discount discount)
        {
            await _unitOfWork.BeginTransaction();
            try
            {
                discount.Id = Guid.NewGuid();
                await _unitOfWork.Discounts.Create(discount);
                foreach (var discountHotel in discount.DiscountHotel)
                {                        
                    discountHotel.DiscountId = discount.Id;
                    discountHotel.Hotel = null;
                    await _unitOfWork.Discounts.CreateDiscountHotel(discountHotel);
                }

                await _unitOfWork.CommitTransaction();  
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransaction();
                throw new ApplicationException("Error creating hotel", ex);
            }
        }

        public async Task CreateDiscountTour(Discount discount)
        {
            await _unitOfWork.BeginTransaction();
            try
            {
                discount.Id = Guid.NewGuid();
                await _unitOfWork.Discounts.Create(discount);
                foreach (var discountTour in discount.DiscountTour)
                {
                    discountTour.DiscountId = discount.Id;
                    discountTour.Tour = null;
                    await _unitOfWork.Discounts.CreateDiscountTour(discountTour);
                }

                await _unitOfWork.CommitTransaction();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransaction();
                throw new ApplicationException("Error creating hotel", ex);
            }
        }

        public async Task<IEnumerable<Discount>> GetByHotel(Guid hotelId, decimal price)
        {
            var hotel = await _unitOfWork.Hotels.GetById(hotelId);
            if (hotel == null)
            {
                throw new ArgumentException("hotel not exist");
            }
            var date = DateTime.Now;
            var discounts = await _unitOfWork.Discounts.GetByHotel(hotelId, date, price);
            return discounts;
        }

        public async Task<Discount?> GetById(Guid id)
        {
            var discount = await _unitOfWork.Discounts.GetById(id);
            return discount;
        }

        public async Task<IEnumerable<Discount>> GetByTour(Guid tourId, decimal price)
        {
            var tour = await _unitOfWork.Tours.GetById(tourId);
            if (tour == null)
            {
                throw new ArgumentException("tour not exist");
            }
            var date = DateTime.Now;
            var discounts = await _unitOfWork.Discounts.GetByTour(tourId, date, price);
            return discounts;
        }

        public async Task<IEnumerable<Discount>> GetByUser(Guid userId)
        {
            var user = await _unitOfWork.Users.GetUserById(userId);
            if (user == null)
            {
                throw new ArgumentException("user not exist");
            }

            var discounts = await _unitOfWork.Discounts.GetByUser(userId);
            return discounts;
        }
    }
}
