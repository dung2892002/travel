using Travel.Core.Interfaces.IRepositories;

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
        IRoomRepository Rooms { get; }
        IBookingRoomRepository BookingsRoom { get; }
        ITourRepository Tours { get; }
        IBookingTourRepository BookingsTour { get; }
        IReviewRepository Reviews { get; }
        IFavouriteRepository Favorites { get; }
        IFacilityRepository Facilities { get; }
        IDiscountRepository Discounts { get; }
        IPaymentRepository Payments { get; }
        IStatiscalRepository Statiscals { get; }
        Task<int> CompleteAsync();
        Task BeginTransaction();
        Task CommitTransaction();
        Task RollbackTransaction();
        new void Dispose();
    }
}
