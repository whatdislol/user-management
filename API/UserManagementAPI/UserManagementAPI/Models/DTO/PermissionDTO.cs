namespace UserManagementAPI.Models.DTO
{
    public class PermissionDTO
    {
        public Guid Id { get; set; }
        public required string PermissionName { get; set; }
    }
}
