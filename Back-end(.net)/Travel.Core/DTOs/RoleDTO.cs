namespace Travel.Core.DTOs
{
    public class RoleDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int RoleValue { get; set; }
    }
}
