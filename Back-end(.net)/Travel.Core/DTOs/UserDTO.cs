using Travel.Core.Entities;

namespace Travel.Core.DTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Fullname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string DisplayName {  get; set; } = string.Empty;
        public string AvatarImage { get; set; } = string.Empty;

        public List<RoleDTO> Roles { get; set; } = new List<RoleDTO>();
    }
}
