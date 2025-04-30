namespace UserManagementAPI.Models.DTO
{
    public class UpdateRolePermissionsRequestDTO
    {
        public Guid RoleId { get; set; }
        public List<Guid> PermissionIds { get; set; } = new();
    }
}
