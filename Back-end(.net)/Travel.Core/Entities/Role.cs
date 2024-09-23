namespace Travel.Core.Entities
{
    public class Role
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public int RoleValue { get; set; }

        public virtual ICollection<UserRole> UserRole { get; set; } = new List<UserRole>();
    }
}
