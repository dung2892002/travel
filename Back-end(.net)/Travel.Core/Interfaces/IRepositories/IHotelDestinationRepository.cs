﻿using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IRepositories
{
    public interface IHotelDestinationRepository
    {
        Task AddRange(IEnumerable<HotelDestination> hotelDestinations);
    }
}
