﻿using Travel.Core.Interfaces.IRepositories;

namespace Travel.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IRoleRepository Roles { get; }
        IUserRoleRepository UserRoles { get; }
        IImageRepository Images { get; }
        IProvinceRepository Provinces { get; }
        ICityRepository Cities { get; }
        IDestinationRepository Destinations { get; }
        IHotelRepository Hotels { get; }
        IHotelDestinationRepository HotelsDestination { get; }
        IRoomRepository Room { get; }
        Task<int> CompleteAsync();
        Task BeginTransaction();
        Task CommitTransaction();
        Task RollbackTransaction();
        new void Dispose();
    }
}
