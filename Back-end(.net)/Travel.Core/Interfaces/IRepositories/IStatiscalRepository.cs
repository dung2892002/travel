using Travel.Core.DTOs;

namespace Travel.Core.Interfaces.IRepositories
{
    public interface IStatiscalRepository
    {
        Task<StatisticalResponse> AdminStatiscal(StatisticalRequest request);
        Task<StatisticalResponse> HotelStatiscal(StatisticalRequest request);
        Task<StatisticalResponse> TourStatiscal(StatisticalRequest request);
    }
}
