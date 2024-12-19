using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Travel.Core.Interfaces;
using Travel.Core.Interfaces.IRepositories;

namespace Travel.Infrastructure.Data
{
    public class UnitOfWork(TravelDbContext dbContext,
                      IUserRepository userRepository,
                      IRoleRepository roleRepository,
                      IUserRoleRepository userRoleRepository,
                      IImageRepository imageRepository,
                      IDestinationRepository destinationRepository,
                      IProvinceRepository provinceRepository,
                      ICityRepository cityRepository,
                      IHotelRepository hotelRepository,
                      IHotelDestinationRepository hotelDestinationRepository,
                      IRoomRepository roomRepository,
                      IBookingRoomRepository bookingRoomRepository,
                      ITourRepository tourRepository,
                      IBookingTourRepository bookingTourRepository,
                      IReviewRepository reviewRepository,
                      IFavouriteRepository favouriteRepository,
                      IFacilityRepository facilityRepository,
                      IDiscountRepository discountRepository,
                      IPaymentRepository paymentRepository,
                      IStatiscalRepository statiscalRepository) : IUnitOfWork, IRepository, IDisposable
    {
        private readonly TravelDbContext _dbContext = dbContext;
        private IDbContextTransaction? _transaction;

        public IUserRepository Users { get; } = userRepository;
        public IRoleRepository Roles { get; } = roleRepository;
        public IUserRoleRepository UserRoles { get; } = userRoleRepository;
        public IImageRepository Images { get; } = imageRepository;
        public IDestinationRepository Destinations { get; } = destinationRepository;
        public IProvinceRepository Provinces { get; } = provinceRepository;
        public ICityRepository Cities {  get; } = cityRepository;
        public IHotelRepository Hotels {  get; } = hotelRepository;
        public IHotelDestinationRepository HotelsDestination {  get; } = hotelDestinationRepository;
        public IRoomRepository Rooms { get; } = roomRepository;
        public IBookingRoomRepository BookingsRoom { get; } = bookingRoomRepository;
        public ITourRepository Tours { get; } = tourRepository;
        public IBookingTourRepository BookingsTour { get; }  = bookingTourRepository;
        public IReviewRepository Reviews { get; } = reviewRepository;
        public IFavouriteRepository Favorites { get; } = favouriteRepository;
        public IFacilityRepository Facilities { get; } = facilityRepository;
        public IDiscountRepository Discounts { get; } = discountRepository;
        public IPaymentRepository Payments { get; } = paymentRepository;
        public IStatiscalRepository Statiscals { get; } = statiscalRepository;
        public async Task BeginTransaction()
        {
            if (_transaction != null)
            {
                throw new InvalidOperationException("A transaction is already in progress.");
            }
            _transaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitTransaction()
        {
            try
            {
                await _transaction.CommitAsync();
            } catch
            {
                await RollbackTransaction();
                throw;
            }
            finally
            {
                if ( _transaction != null )
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackTransaction()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();

                _transaction = null;
            }
        }
        public async Task<int> CompleteAsync()
        {
            try
            {
                var result = await _dbContext.SaveChangesAsync();
                Console.WriteLine($"Number of state changes: {result}");
                return result;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
                throw new DbUpdateException("no change"); 
            }
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        
    }
}
