using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Travel.Core.DTOs;
using Travel.Core.Interfaces;
using Travel.Core.Interfaces.IServices;

namespace Travel.Core.Services
{
    public class PaymentService(IUnitOfWork unitOfWork) : IPaymentService, IService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        private const string VNPayUrl = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
        private const string TmnCode = "X0B56OW7";
        private const string HashSecret = "VD9CIFN9L4TVDKEXSSO45X6OMY07WWE3"; 

        public async Task<string> ProcessPaymentForRoom(PaymentRequest request)
        {
            return await ProcessPayment(request);
        }

        public async Task<string> ProcessPaymentForTour(PaymentRequest request)
        {
            return await ProcessPayment(request);
        }

        private async Task<string> ProcessPayment(PaymentRequest request)
        {
            VnPayLibrary vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", "2.1.0");
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", TmnCode);
            vnpay.AddRequestData("vnp_Amount", (request.Amount * 100).ToString("0"));
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan cho boooking:" + request.BookingId.ToString());
            vnpay.AddRequestData("vnp_OrderType", "other");
            vnpay.AddRequestData("vnp_ReturnUrl", request.ReturnUrl);
            vnpay.AddRequestData("vnp_TxnRef", request.BookingId.ToString());

            string paymentUrl = vnpay.CreateRequestUrl(VNPayUrl, HashSecret);
                        
            return paymentUrl; 
        }
                     

        public async Task<bool> PaymentExecute(IQueryCollection collections, int type)
        {
            var vnpay = new VnPayLibrary();
            foreach (var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }

            var vnp_BookingId = Guid.Parse(vnpay.GetResponseData("vnp_TxnRef"));
            var vnp_TransactionId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            var vnp_SecureHash = collections.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
            var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");

            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, HashSecret);

            if (!checkSignature)
            {
               return false;
            }
            return await UpdateBooking(vnp_BookingId, type);
        }

        private async Task<bool> UpdateBooking(Guid id, int type)
        {
            if (type == 0)
            {
                var booking = await _unitOfWork.BookingRoom.GetById(id);
                booking.Status = 1;
                var result = await _unitOfWork.CompleteAsync();
                return result > 0;
            }
            return false;
        }
    }
}
