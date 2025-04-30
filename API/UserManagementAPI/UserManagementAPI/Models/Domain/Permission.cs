namespace UserManagementAPI.Models.Domain
{
    public class Permission
    {
        public Guid Id { get; set; }
        public required string PermissionName { get; set; }

        public ICollection<Role> Roles { get; set; } = new List<Role>();
    }
}
