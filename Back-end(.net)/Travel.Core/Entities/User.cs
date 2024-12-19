using System.Text.Json.Serialization;

namespace Travel.Core.Entities;

public class User
{
    public Guid Id { get; set; }

    public string Fullname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public string AvatarImage { get; set; } = null!;

    public bool IsLocked { get; set; }

    [JsonIgnore]
    public virtual ICollection<BookingRoom> BookingRoom { get; set; } = new List<BookingRoom>();

    [JsonIgnore]
    public virtual ICollection<BookingTour> BookingTour { get; set; } = new List<BookingTour>();

    [JsonIgnore]
    public virtual ICollection<Discount> Discount { get; set; } = new List<Discount>();

    [JsonIgnore]
    public virtual ICollection<Favourite> Favourite { get; set; } = new List<Favourite>();

    [JsonIgnore]
    public virtual ICollection<Hotel> Hotel { get; set; } = new List<Hotel>();

    [JsonIgnore]
    public virtual ICollection<Review> Review { get; set; } = new List<Review>();

    [JsonIgnore]
    public virtual ICollection<Tour> Tour { get; set; } = new List<Tour>();

    [JsonIgnore]
    public virtual ICollection<UserRole> UserRole { get; set; } = new List<UserRole>();
    public virtual Wallet? Wallet { get; set; }
}
