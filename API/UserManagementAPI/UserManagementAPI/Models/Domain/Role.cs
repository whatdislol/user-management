namespace UserManagementAPI.Models.Domain
{
    public class Role
    {
        public Guid Id { get; set; }
        public required string RoleName { get; set; }

        public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
        public ICollection<User> Users { get; set; } = new List<User>();

    }
}
