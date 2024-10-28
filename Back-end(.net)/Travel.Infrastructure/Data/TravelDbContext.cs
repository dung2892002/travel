using Microsoft.EntityFrameworkCore;
using Travel.Core.Entities;


namespace Travel.Infrastructure.Data;

public partial class TravelDbContext(DbContextOptions<TravelDbContext> options) : DbContext(options)
{
    public virtual DbSet<Activity> Activity { get; set; }

    public virtual DbSet<Airline> Airline { get; set; }

    public virtual DbSet<Airport> Airport { get; set; }

    public virtual DbSet<BookingFlight> BookingFlight { get; set; }

    public virtual DbSet<BookingRoom> BookingRoom { get; set; }

    public virtual DbSet<BookingTour> BookingTour { get; set; }

    public virtual DbSet<BookingTrain> BookingTrain { get; set; }

    public virtual DbSet<City> City { get; set; }

    public virtual DbSet<Province> Province { get; set; }

    public virtual DbSet<Destination> Destination { get; set; }

    public virtual DbSet<Favourite> Favourite { get; set; }

    public virtual DbSet<Flight> Flight { get; set; }

    public virtual DbSet<Hotel> Hotel { get; set; }

    public virtual DbSet<HotelDestination> HotelDestination { get; set; }

    public virtual DbSet<Image> Image { get; set; }

    public virtual DbSet<Review> Review { get; set; }

    public virtual DbSet<Role> Role { get; set; }

    public virtual DbSet<Room> Room { get; set; }

    public virtual DbSet<TimeSlot> TimeSlot { get; set; }

    public virtual DbSet<Tour> Tour { get; set; }

    public virtual DbSet<TourDay> TourDay { get; set; }

    public virtual DbSet<TourDestination> TourDestination { get; set; }

    public virtual DbSet<Train> Train { get; set; }

    public virtual DbSet<TrainOperator> TrainOperator { get; set; }

    public virtual DbSet<TrainStation> TrainStation { get; set; }

    public virtual DbSet<User> User { get; set; }

    public virtual DbSet<UserRole> UserRole { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Activity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("activity");

            entity.HasIndex(e => e.TimeSlotId, "FK_activity_TimeSlotId");

            entity.Property(e => e.Description).HasMaxLength(1500);
            entity.Property(e => e.Location).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.TimeSlot).WithMany(p => p.Activity)
                .HasForeignKey(d => d.TimeSlotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_activity_TimeSlotId");
        });

        modelBuilder.Entity<Airline>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("airline");

            entity.Property(e => e.Code).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Airport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("airport");

            entity.HasIndex(e => e.CityId, "FK_airport_CityId");

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.City).WithMany(p => p.Airport)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_airport_CityId");
        });

        modelBuilder.Entity<BookingFlight>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("booking_flight");

            entity.HasIndex(e => e.FlightId, "FK_booking_flight_FlightId");

            entity.HasIndex(e => e.UserId, "FK_booking_flight_UserId");

            entity.Property(e => e.CancelReason).HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CustomerIdentityNumber).HasMaxLength(255);
            entity.Property(e => e.CustomerName).HasMaxLength(255);
            entity.Property(e => e.Price).HasPrecision(19, 2);

            entity.HasOne(d => d.Flight).WithMany(p => p.BookingFlight)
                .HasForeignKey(d => d.FlightId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_booking_flight_FlightId");

            entity.HasOne(d => d.User).WithMany(p => p.BookingFlight)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_booking_flight_UserId");
        });

        modelBuilder.Entity<BookingRoom>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("booking_room");

            entity.HasIndex(e => e.RoomId, "FK_booking_room_RoomId");

            entity.HasIndex(e => e.UserId, "FK_booking_room_UserId");

            entity.Property(e => e.CancelReason).HasMaxLength(255);
            entity.Property(e => e.CheckInDate).HasColumnType("datetime");
            entity.Property(e => e.CheckOutDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CustomerName).HasMaxLength(255);
            entity.Property(e => e.Price).HasPrecision(19, 2);

            entity.HasOne(d => d.Room).WithMany(p => p.BookingRoom)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_booking_room_RoomId");

            entity.HasOne(d => d.User).WithMany(p => p.BookingRoom)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_booking_room_UserId");
        });

        modelBuilder.Entity<BookingTour>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("booking_tour");

            entity.HasIndex(e => e.TourId, "FK_booking_tour_TourId");

            entity.HasIndex(e => e.UserId, "FK_booking_tour_UserId");

            entity.Property(e => e.CancelReason).HasMaxLength(255);
            entity.Property(e => e.Price).HasPrecision(19, 2);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Tour).WithMany(p => p.BookingTour)
                .HasForeignKey(d => d.TourId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_booking_tour_TourId");

            entity.HasOne(d => d.User).WithMany(p => p.BookingTour)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_booking_tour_UserId");
        });

        modelBuilder.Entity<BookingTrain>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("booking_train");

            entity.HasIndex(e => e.TrainId, "FK_booking_train_TrainId");

            entity.HasIndex(e => e.UserId, "FK_booking_train_UserId");

            entity.Property(e => e.CancelReason).HasMaxLength(255);
            entity.Property(e => e.CustomerIdentityNumber).HasMaxLength(255);
            entity.Property(e => e.CustomerName).HasMaxLength(255);
            entity.Property(e => e.Price).HasPrecision(19, 2);

            entity.HasOne(d => d.Train).WithMany(p => p.BookingTrain)
                .HasForeignKey(d => d.TrainId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_booking_train_TrainId");

            entity.HasOne(d => d.User).WithMany(p => p.BookingTrain)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_booking_train_UserId");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("city");

            entity.HasIndex(e => e.ProvinceId, "FK_city_ProvinceId");

            entity.Property(e => e.Image).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Province).WithMany(p => p.City)
                .HasForeignKey(d => d.ProvinceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_city_ProvinceId");
        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("province");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Destination>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("destination");

            entity.HasIndex(e => e.CityId, "FK_destination_CityId");

            entity.Property(e => e.Description).HasMaxLength(2500);
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.City).WithMany(p => p.Destination)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_destination_CityId");
        });

        modelBuilder.Entity<Favourite>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("favourite");

            entity.HasIndex(e => e.CityId, "FK_favourite_CityId");

            entity.HasIndex(e => e.DestinationId, "FK_favourite_DestinationId");

            entity.HasIndex(e => e.HotelId, "FK_favourite_HotelId");

            entity.HasIndex(e => e.TourId, "FK_favourite_TourId");

            entity.HasIndex(e => e.UserId, "FK_favourite_UserId");

            entity.HasOne(d => d.City).WithMany(p => p.Favourite)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK_favourite_CityId");

            entity.HasOne(d => d.Destination).WithMany(p => p.Favourite)
                .HasForeignKey(d => d.DestinationId)
                .HasConstraintName("FK_favourite_DestinationId");

            entity.HasOne(d => d.Hotel).WithMany(p => p.Favourite)
                .HasForeignKey(d => d.HotelId)
                .HasConstraintName("FK_favourite_HotelId");

            entity.HasOne(d => d.Tour).WithMany(p => p.Favourite)
                .HasForeignKey(d => d.TourId)
                .HasConstraintName("FK_favourite_TourId");

            entity.HasOne(d => d.User).WithMany(p => p.Favourite)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_favourite_UserId");
        });

        modelBuilder.Entity<Flight>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("flight");

            entity.HasIndex(e => e.AirlineId, "FK_Flight_AirlineId");

            entity.HasIndex(e => e.ArrivalAirportId, "FK_Flight_ArrivalAirportId");

            entity.HasIndex(e => e.DepartureAirportId, "FK_Flight_DepartureAirportId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code).HasMaxLength(255);
            entity.Property(e => e.DepartureTime).HasColumnType("datetime");

            entity.HasOne(d => d.Airline).WithMany(p => p.Flight)
                .HasForeignKey(d => d.AirlineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Flight_AirlineId");

            entity.HasOne(d => d.ArrivalAirport).WithMany(p => p.FlightArrivalAirport)
                .HasForeignKey(d => d.ArrivalAirportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Flight_ArrivalAirportId");

            entity.HasOne(d => d.DepartureAirport).WithMany(p => p.FlightDepartureAirport)
                .HasForeignKey(d => d.DepartureAirportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Flight_DepartureAirportId");
        });

        modelBuilder.Entity<Hotel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("hotel");

            entity.HasIndex(e => e.UserId, "FK_hotel_UserId");

            entity.Property(e => e.CheckInTime).HasColumnType("time");
            entity.Property(e => e.CheckOutTime).HasColumnType("time");
            entity.Property(e => e.Description).HasMaxLength(1500);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.PhoneNumber).HasMaxLength(255);

            entity.HasOne(d => d.User).WithMany(p => p.Hotel)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_hotel_UserId");
        });

        modelBuilder.Entity<HotelDestination>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("hotel_destination");

            entity.HasIndex(e => e.DestinationId, "FK_hotel_destination_DestinationId");

            entity.HasIndex(e => e.HotelId, "FK_hotel_destination_HotelId");

            entity.HasOne(d => d.Destination).WithMany(p => p.HotelDestination)
                .HasForeignKey(d => d.DestinationId)
                .HasConstraintName("FK_hotel_destination_DestinationId");

            entity.HasOne(d => d.Hotel).WithMany(p => p.HotelDestination)
                .HasForeignKey(d => d.HotelId)
                .HasConstraintName("FK_hotel_destination_HotelId");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("image");

            entity.HasIndex(e => e.TourId, "FK_image_TourId");

            entity.HasIndex(e => e.DestinationId, "FK_image_DestinationId");

            entity.HasIndex(e => e.HotelId, "FK_image_HotelId");

            entity.HasIndex(e => e.RoomId, "FK_image_RoomId");

            entity.HasIndex(e => e.ReviewId, "FK_image_ReviewId");

            entity.Property(e => e.Path).HasMaxLength(255);

            entity.HasOne(d => d.Tour).WithMany(p => p.Image)
                .HasForeignKey(d => d.TourId)
                .HasConstraintName("FK_image_TourId");

            entity.HasOne(d => d.Destination).WithMany(p => p.Image)
                .HasForeignKey(d => d.DestinationId)
                .HasConstraintName("FK_image_DestinationId");

            entity.HasOne(d => d.Hotel).WithMany(p => p.Image)
                .HasForeignKey(d => d.HotelId)
                .HasConstraintName("FK_image_HotelId");

            entity.HasOne(d => d.Room).WithMany(p => p.Image)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK_image_RoomId");

            entity.HasOne(d => d.Review).WithMany(p => p.Image)
                .HasForeignKey(d => d.ReviewId)
                .HasConstraintName("FK_image_ReviewId")
                .OnDelete(DeleteBehavior.Cascade);                
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("review");

            entity.HasIndex(e => e.DestinationId, "FK_Review_DestinationId");

            entity.HasIndex(e => e.HotelId, "FK_Review_HotelId");

            entity.HasIndex(e => e.TourId, "FK_Review_TourId");

            entity.HasIndex(e => e.UserId, "FK_Review_UserId");

            entity.Property(e => e.Description).HasMaxLength(255);

            entity.HasOne(d => d.Destination).WithMany(p => p.Review)
                .HasForeignKey(d => d.DestinationId)
                .HasConstraintName("FK_Review_DestinationId");

            entity.HasOne(d => d.Hotel).WithMany(p => p.Review)
                .HasForeignKey(d => d.HotelId)
                .HasConstraintName("FK_Review_HotelId");

            entity.HasOne(d => d.Tour).WithMany(p => p.Review)
                .HasForeignKey(d => d.TourId)
                .HasConstraintName("FK_Review_TourId");

            entity.HasOne(d => d.User).WithMany(p => p.Review)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Review_UserId");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("role");

            entity.Property(e => e.Name).HasMaxLength(45);
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("room");

            entity.HasIndex(e => e.HotelId, "FK_room_HotelId");

            entity.Property(e => e.Area).HasPrecision(10, 2);
            entity.Property(e => e.Dirention).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Price).HasPrecision(19, 2);

            entity.HasOne(d => d.Hotel).WithMany(p => p.Room)
                .HasForeignKey(d => d.HotelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_room_HotelId");
        });

        modelBuilder.Entity<TimeSlot>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("time_slot");

            entity.HasIndex(e => e.TourDayId, "FK_time_slot_TourDayId");

            entity.Property(e => e.Description).HasMaxLength(1500);

            entity.HasOne(d => d.TourDay).WithMany(p => p.TimeSlot)
                .HasForeignKey(d => d.TourDayId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_time_slot_TourDayId");
        });

        modelBuilder.Entity<Tour>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tour");

            entity.HasIndex(e => e.DepartureCityId, "FK_tour_DepartureCityId");

            entity.Property(e => e.Code).HasMaxLength(255);
            entity.Property(e => e.Description).HasMaxLength(2500);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.PriceAdultPeople)
                .HasPrecision(19, 2)
                .HasComment("tu 10 tuoi tro len");
            entity.Property(e => e.PricePrimaryChildren)
                .HasPrecision(19, 2)
                .HasComment("tre duoi 10 tuoi");
            entity.Property(e => e.PriceToddler)
                .HasPrecision(19, 2)
                .HasComment("tre duoi 5 tuoi");

            entity.HasOne(d => d.DepartureCity).WithMany(p => p.Tour)
                .HasForeignKey(d => d.DepartureCityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tour_DepartureCityId");
        });

        modelBuilder.Entity<TourDay>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tour_day");

            entity.HasIndex(e => e.TourId, "FK_tour_day_TourId");

            entity.Property(e => e.Description).HasMaxLength(1500);

            entity.HasOne(d => d.Tour).WithMany(p => p.TourDay)
                .HasForeignKey(d => d.TourId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tour_day_TourId");
        });

        modelBuilder.Entity<TourDestination>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tour_destination");

            entity.HasIndex(e => e.DestinationId, "FK_tour_destination_DestinationId");

            entity.HasIndex(e => e.TourId, "FK_tour_destination_TourId");

            entity.HasOne(d => d.Destination).WithMany(p => p.TourDestination)
                .HasForeignKey(d => d.DestinationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tour_destination_DestinationId");

            entity.HasOne(d => d.Tour).WithMany(p => p.TourDestination)
                .HasForeignKey(d => d.TourId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tour_destination_TourId");
        });

        modelBuilder.Entity<Train>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("train");

            entity.HasIndex(e => e.ArrivalTrainStationId, "FK_train_ArrivalTrainStationId");

            entity.HasIndex(e => e.DepartureTrainStationId, "FK_train_DepartureTrainStationId");

            entity.HasIndex(e => e.TrainOperatorId, "FK_train_TrainOperatorId");

            entity.Property(e => e.Code).HasMaxLength(255);
            entity.Property(e => e.DepartureTime).HasColumnType("datetime");

            entity.HasOne(d => d.ArrivalTrainStation).WithMany(p => p.TrainArrivalTrainStation)
                .HasForeignKey(d => d.ArrivalTrainStationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_train_ArrivalTrainStationId");

            entity.HasOne(d => d.DepartureTrainStation).WithMany(p => p.TrainDepartureTrainStation)
                .HasForeignKey(d => d.DepartureTrainStationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_train_DepartureTrainStationId");

            entity.HasOne(d => d.TrainOperator).WithMany(p => p.Train)
                .HasForeignKey(d => d.TrainOperatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_train_TrainOperatorId");
        });

        modelBuilder.Entity<TrainOperator>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("train_operator");

            entity.Property(e => e.Code).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<TrainStation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("train_station");

            entity.HasIndex(e => e.CityId, "FK_train_station_CityId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.City).WithMany(p => p.TrainStation)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_train_station_CityId");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user");

            entity.Property(e => e.AvatarImage).HasMaxLength(255);
            entity.Property(e => e.DisplayName).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Fullname).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(255);
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user_role");

            entity.HasIndex(e => e.RoleId, "FK_role_idx");

            entity.HasIndex(e => e.UserId, "FK_user_idx");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRole)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_role");

            entity.HasOne(d => d.User).WithMany(p => p.UserRole)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_user");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
