using Travel.Core.DTOs;

namespace Travel.Core.Interfaces.IServices
{
    public interface IStatiscalService
    {
        Task<StatisticalResponse> AdminStatiscal(StatisticalRequest request);
        Task<StatisticalResponse> HotelStatiscal(StatisticalRequest request);
        Task<StatisticalResponse> TourStatiscal(StatisticalRequest request);
    }
}
