﻿using Microsoft.EntityFrameworkCore;
using Travel.Core.Entities;


namespace Travel.Infrastructure.Data;

public partial class TravelDbContext(DbContextOptions<TravelDbContext> options) : DbContext(options)
{
    public virtual DbSet<Activity> Activity { get; set; }

    public virtual DbSet<BookingRoom> BookingRoom { get; set; }

    public virtual DbSet<BookingTour> BookingTour { get; set; }

    public virtual DbSet<City> City { get; set; }

    public virtual DbSet<Province> Province { get; set; }

    public virtual DbSet<Destination> Destination { get; set; }

    public virtual DbSet<Facility> Facility {  get; set; }

    public virtual DbSet<Favourite> Favourite { get; set; }

    public virtual DbSet<Hotel> Hotel { get; set; }

    public virtual DbSet<HotelDestination> HotelDestination { get; set; }
    public virtual DbSet<HotelFacility> HotelFacility { get; set; }

    public virtual DbSet<Image> Image { get; set; }

    public virtual DbSet<Review> Review { get; set; }

    public virtual DbSet<Role> Role { get; set; }

    public virtual DbSet<Room> Room { get; set; }
    public virtual DbSet<RoomFacility> RoomFacility { get; set; }

    public virtual DbSet<Tour> Tour { get; set; }

    public virtual DbSet<TourDay> TourDay { get; set; }

    public virtual DbSet<TourCity> TourCity { get; set; }

    public virtual DbSet<User> User { get; set; }

    public virtual DbSet<UserRole> UserRole { get; set; }

    public virtual DbSet<Discount> Discount { get; set; }
    public virtual DbSet<DiscountHotel> DiscountHotel {  get; set; }
    public virtual DbSet<DiscountTour> DiscountTour {  get; set; }
    public virtual DbSet<Payment> Payment { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Activity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("activity");

            entity.HasIndex(e => e.TourDayId, "FK_activity_TourDayId_idx");

            entity.Property(e => e.Description).HasMaxLength(1500);
            entity.Property(e => e.Location).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.TourDay).WithMany(p => p.Activity)
                .HasForeignKey(d => d.TourDayId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_activity_TourDayId");
        });

        modelBuilder.Entity<BookingRoom>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("booking_room");

            entity.HasIndex(e => e.RoomId, "FK_booking_room_RoomId");

            entity.HasIndex(e => e.UserId, "FK_booking_room_UserId");

            entity.Property(e => e.CancelReason).HasMaxLength(255);
            entity.Property(e => e.ContactName).HasMaxLength(255);
            entity.Property(e => e.ContactEmail).HasMaxLength(100);
            entity.Property(e => e.CheckInDate).HasColumnType("datetime");
            entity.Property(e => e.CheckOutDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.ContactPhone).HasMaxLength(45);
            entity.Property(e => e.Price).HasPrecision(19, 2);

            entity.HasOne(d => d.Room).WithMany(p => p.BookingRoom)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_booking_room_RoomId");

            entity.HasOne(d => d.User).WithMany(p => p.BookingRoom)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_booking_room_UserId");

            entity.HasOne(d => d.Discount).WithMany(p => p.BookingRoom)
                .HasForeignKey(d => d.DiscountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_booking_room_DiscountId");
        });

        modelBuilder.Entity<BookingTour>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("booking_tour");

            entity.HasIndex(e => e.TourId, "FK_booking_tour_TourId");

            entity.HasIndex(e => e.UserId, "FK_booking_tour_UserId");

            entity.Property(e => e.CancelReason).HasMaxLength(255);
            entity.Property(e => e.ContactName).HasMaxLength(255);
            entity.Property(e => e.ContactEmail).HasMaxLength(100);
            entity.Property(e => e.ContactPhone).HasMaxLength(45);

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

            entity.HasOne(d => d.Discount).WithMany(p => p.BookingTour)
                .HasForeignKey(d => d.DiscountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_booking_tour_DiscountId");
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

        modelBuilder.Entity<Facility>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("facility");
            entity.Property(e =>e.Name).HasMaxLength(255);
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

        

        modelBuilder.Entity<Hotel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("hotel");

            entity.HasIndex(e => e.UserId, "FK_hotel_UserId");
            entity.HasIndex(e => e.UserId, "FK_hotel_CityId");

            entity.Property(e => e.CheckInTime).HasColumnType("time");
            entity.Property(e => e.CheckOutTime).HasColumnType("time");
            entity.Property(e => e.Description).HasMaxLength(1500);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(255);

            entity.HasOne(d => d.User).WithMany(p => p.Hotel)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_hotel_UserId");

            entity.HasOne(d => d.City).WithMany(p => p.Hotel)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK_hotel_CityId");
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

        modelBuilder.Entity<HotelFacility>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("hotel_facility");

            entity.HasIndex(e => e.FacilityId, "FK_hotel_facility_FacilityId");

            entity.HasIndex(e => e.HotelId, "FK_hotel_destination_HotelId");

            entity.HasOne(d => d.Facility).WithMany(p => p.HotelFacility)
                .HasForeignKey(d => d.FacilityId)
                .HasConstraintName("FK_hotel_facility_FacilityId");

            entity.HasOne(d => d.Hotel).WithMany(p => p.HotelFacility)
                .HasForeignKey(d => d.HotelId)
                .HasConstraintName("FK_hotel_facility_HotelId");
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

            entity.Property(e => e.Description).HasMaxLength(1500);

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

        modelBuilder.Entity<RoomFacility>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("room_facility");

            entity.HasIndex(e => e.FacilityId, "FK_room_facility_FacilityId");

            entity.HasIndex(e => e.RoomId, "FK_room_facility_RoomId");

            entity.HasOne(d => d.Facility).WithMany(p => p.RoomFacility)
                .HasForeignKey(d => d.FacilityId)
                .HasConstraintName("FK_room_facility_FacilityId");

            entity.HasOne(d => d.Room).WithMany(p => p.RoomFacility)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK_room_facility_RoomId");
        });

        modelBuilder.Entity<Tour>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tour");

            entity.HasIndex(e => e.DepartureCityId, "FK_tour_DepartureCityId");

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

            entity.HasOne(d => d.User).WithMany(p => p.Tour)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tour_UserId");
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

        modelBuilder.Entity<TourCity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tour_city");

            entity.HasIndex(e => e.CityId, "FK_tour_city_CityId_idx");

            entity.HasIndex(e => e.TourId, "FK_tour_city_TourId_idx");

            entity.HasOne(d => d.City).WithMany(p => p.TourCity)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tour_city_CityId");

            entity.HasOne(d => d.Tour).WithMany(p => p.TourCity)
                .HasForeignKey(d => d.TourId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tour_city_TourId");
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

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("discount");
            entity.Property(e => e.MinPrice).HasPrecision(19, 2);
            entity.Property(e => e.MaxDiscount).HasPrecision(19, 2);
            entity.Property(e => e.Start).HasColumnType("datetime");
            entity.Property(e => e.End).HasColumnType("datetime");

            entity.HasIndex(e => e.UserId, "FK_discount_UserId_idx");

            entity.HasOne(d => d.User).WithMany(p => p.Discount)
              .HasForeignKey(d => d.UserId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("FK_discount_UserId");
        });

        modelBuilder.Entity<DiscountHotel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("discount_hotel");

            entity.HasIndex(e => e.DiscountId, "FK_discount_hotel_DiscountId_idx");

            entity.HasIndex(e => e.HotelId, "FK_discount_hotel_HotelId_idx");

            entity.HasOne(d => d.Discount).WithMany(p => p.DiscountHotel)
               .HasForeignKey(d => d.DiscountId)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK_discount_hotel_DiscountId");

            entity.HasOne(d => d.Hotel).WithMany(p => p.DiscountHotel)
              .HasForeignKey(d => d.HotelId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("FK_discount_hotel_HotelId");
        });

        modelBuilder.Entity<DiscountTour>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("discount_tour");

            entity.HasIndex(e => e.DiscountId, "FK_discount_tour_DiscountId_idx");

            entity.HasIndex(e => e.TourId, "FK_discount_tour_TourId_idx");

            entity.HasOne(d => d.Discount).WithMany(p => p.DiscountTour)
               .HasForeignKey(d => d.DiscountId)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK_discount_tour_DiscountId");

            entity.HasOne(d => d.Tour).WithMany(p => p.DiscountTour)
              .HasForeignKey(d => d.TourId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("FK_discount_tour_TourId");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("payment");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Amount).HasPrecision(19, 2);

            entity.HasIndex(e => e.BookingRoomId, "FK_payment_BookingRoomId_idx");

            entity.HasIndex(e => e.BookingTourId, "FK_payment_BookingTourId_idx");

            entity.HasOne(d => d.BookingRoom).WithMany(p => p.Payment)
               .HasForeignKey(d => d.BookingRoomId)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK_payment_BookingRoomId");

            entity.HasOne(d => d.BookingTour).WithMany(p => p.Payment)
              .HasForeignKey(d => d.BookingTourId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("FK_payment_BookingTourId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
