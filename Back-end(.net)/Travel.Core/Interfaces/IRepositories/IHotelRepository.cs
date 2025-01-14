﻿using Travel.Core.DTOs;
using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IRepositories
{
    public interface IHotelRepository
    {
        Task<IEnumerable<Hotel>> GetAllHotel();

        Task<Hotel?> GetById(Guid id);

        Task<IEnumerable<Hotel>> GetByPartner(Guid partnerId);

        Task<IEnumerable<Hotel>> GetByDestination(int destinationId);

        Task CreateHotel(Hotel hotel);

        Task CreateHotelDestination(HotelDestination hotelDestination);
        Task DeleteHotelDestination(HotelDestination hotelDestination);
        Task CreateHotelFacility(HotelFacility hotelFacility);
        Task DeleteHotelFacility(HotelFacility hotelFacility);
        Task CreateHotelRefund(Refund refund);
        Task DeleteHotelRefund(Refund refund);

        Task<IEnumerable<Refund>> GetHotelRefundByHotel(Guid hotelId);
        Task<Refund?> GetHotelRefundById(int id);

        Task<PagedResult<SearchHotelResponse>> SearchHotel(SearchHotelRequest request);
    }
}
