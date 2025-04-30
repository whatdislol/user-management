namespace UserManagementAPI.Models.DTO
{
    public class RolePermissionsDTO
    {
        public Guid RoleId { get; set; }
        public required string RoleName { get; set; }
        public List<PermissionCheckboxDTO> Permissions { get; set; } = new();
    }

    public class PermissionCheckboxDTO
    {
        public Guid Id { get; set; }
        public required string PermissionName { get; set; }
        public bool IsSelected { get; set; }
    }
}
