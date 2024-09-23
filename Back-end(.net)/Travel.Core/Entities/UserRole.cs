namespace Travel.Core.Entities
{
    public class UserRole
    {
        public int Id { get; set; }

        public Guid RoleId { get; set; }

        public Guid UserId { get; set; }

        public virtual Role Role { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}
