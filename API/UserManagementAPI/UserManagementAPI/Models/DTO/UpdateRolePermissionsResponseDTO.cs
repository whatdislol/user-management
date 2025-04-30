namespace UserManagementAPI.Models.DTO
{
    public class UpdateRolePermissionsResponseDTO
    {
        public Guid RoleId { get; set; }
        public required string RoleName { get; set; }
        public List<PermissionDTO> Permissions { get; set; } = new List<PermissionDTO>();
    }
}
