using Microsoft.AspNetCore.Http;
using Travel.Core.DTOs;

namespace Travel.Core.Interfaces.IServices
{
    public interface IPaymentService
    {
        Task<string> ProcessPaymentForRoom(PaymentRequest request);
        Task<string> ProcessPaymentForTour(PaymentRequest request);
        Task<bool> PaymentExecute(IQueryCollection collections, int type);
    }
}
