using Travel.Core.Entities;

namespace Travel.Core.DTOs
{
    public class SearchRoomResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public short Quantity { get; set; }

        public short AvailableQuantity { get; set; }

        public decimal Area { get; set; }

        public short MaxAdultPeople { get; set; }

        public short MaxChildrenPeople { get; set; }

        public short SingleBed { get; set; }

        public short DoubleBed { get; set; }

        public string? Dirention { get; set; }

        public Guid HotelId { get; set; }

        public virtual ICollection<Image> Image { get; set; } = new List<Image>();
        public virtual ICollection<RoomFacility> RoomFacility { get; set; } = new List<RoomFacility>();
    }
}
