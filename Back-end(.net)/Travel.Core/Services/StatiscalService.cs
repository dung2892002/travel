using Travel.Core.DTOs;
using Travel.Core.Interfaces;
using Travel.Core.Interfaces.IServices;

namespace Travel.Core.Services
{
    public class StatiscalService(IUnitOfWork unitOfWork) : IStatiscalService, IService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<StatisticalResponse> AdminStatiscal(StatisticalRequest request)
        {
            return await _unitOfWork.Statiscals.AdminStatiscal(request);
        }

        public async Task<StatisticalResponse> HotelStatiscal(StatisticalRequest request)
        {
            return await _unitOfWork.Statiscals.HotelStatiscal(request);
        }

        public async Task<StatisticalResponse> TourStatiscal(StatisticalRequest request)
        {
            return await _unitOfWork.Statiscals.TourStatiscal(request);
        }
    }
}
