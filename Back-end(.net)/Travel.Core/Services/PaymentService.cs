using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Travel.Core.DTOs;
using Travel.Core.Entities;
using Travel.Core.Interfaces;
using Travel.Core.Interfaces.IServices;

namespace Travel.Core.Services
{
    public class PaymentService(IUnitOfWork unitOfWork, IConfiguration configuration) : IPaymentService, IService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IConfiguration _configuration = configuration;

        public async Task<string> ProcessPaymentForRoom(PaymentRequest request)
        {
            var booking = await _unitOfWork.BookingsRoom.GetById(request.BookingId) ?? throw new ArgumentException("booking not exist");
            if (booking.Status != 0)
            {
                throw new InvalidOperationException("Booking has been paid");
            }
            var discount = await _unitOfWork.Discounts.GetById(booking.DiscountId);
            request.Amount = booking.Price - CalculateDiscount(booking.Price, discount);
            return await ProcessPayment(request, 0);
        }

        public async Task<string> ProcessPaymentForTour(PaymentRequest request)
        {
            var booking = await _unitOfWork.BookingsTour.GetById(request.BookingId) ?? throw new ArgumentException("booking not exist");
            if (booking.Status != 0)
            {
                throw new InvalidOperationException("Booking has been paid");
            }

            var discount = await _unitOfWork.Discounts.GetById(booking.DiscountId);
            request.Amount = booking.Price - CalculateDiscount(booking.Price, discount);
            return await ProcessPayment(request, 1);
        }

        private async Task<string> ProcessPayment(PaymentRequest request, int type)
        {
            var TmnCode = _configuration["VnPay:TmnCode"];
            var VNPayUrl = _configuration["VnPay:VNPayUrl"];
            var HashSecret = _configuration["VnPay:HashSecret"];


            VnPayLibrary vnpay = new();
            vnpay.AddRequestData("vnp_Version", "2.1.0");
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", TmnCode);
            vnpay.AddRequestData("vnp_Amount", (request.Amount * 100).ToString("0"));
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", request.BookingId.ToString());
            vnpay.AddRequestData("vnp_OrderType", "other");
            vnpay.AddRequestData("vnp_ReturnUrl", request.ReturnUrl);
            vnpay.AddRequestData("vnp_TxnRef", Guid.NewGuid().ToString());

            string paymentUrl = vnpay.CreateRequestUrl(VNPayUrl, HashSecret);
                        
            return paymentUrl; 
        }
                     

        public async Task<bool> PaymentExecute(IQueryCollection collections, int type)
        {
            var HashSecret = _configuration["VnPay:HashSecret"];
            var vnpay = new VnPayLibrary();
            foreach (var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }

            var paymentId = Guid.Parse(vnpay.GetResponseData("vnp_TxnRef"));
            var transactionId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            var vnp_SecureHash = collections.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
            var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var bookingId = Guid.Parse(vnpay.GetResponseData("vnp_OrderInfo"));
            var amount = Decimal.Parse(vnpay.GetResponseData("vnp_Amount")) / 100;
            var time = DateTime.ParseExact(vnpay.GetResponseData("vnp_PayDate"), "yyyyMMddHHmmss", null);

            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, HashSecret);

            if (!checkSignature)
            {
               return false;
            }

            if (vnp_ResponseCode != "00") 
            {
                return false;
            }

            var payment = new Payment
            {
                Id = paymentId,
                Amount = amount,
                CreatedAt = time,
                TransactionId = transactionId,
                BookingRoomId = null,
                BookingTourId = null
            };

            if (type == 0) payment.BookingRoomId = bookingId;
            else payment.BookingTourId = bookingId;

            await _unitOfWork.Payments.Create(payment);
            await _unitOfWork.CompleteAsync();

            await UpdateBooking(bookingId, type);
            return true;
        }


        private async Task<bool> UpdateBooking(Guid bookingId, int type)
        {
            if (type == 0)
            {
                var booking = await _unitOfWork.BookingsRoom.GetById(bookingId);
                booking.Status = 1;
                var result = await _unitOfWork.CompleteAsync();
                return result > 0;
            } else
            {
                var booking = await _unitOfWork.BookingsTour.GetById(bookingId);
                booking.Status = 1;
                var result = await _unitOfWork.CompleteAsync();
                return result > 0;
            }
        }

        private static decimal CalculateDiscount(decimal amount, Discount discount)
        {
            if (discount == null) return 0;

            var discountValue = Math.Round(discount.Percent * amount / 100);
            return discountValue > discount.MaxDiscount ? discount.MaxDiscount : discountValue;
        }
    }
}
