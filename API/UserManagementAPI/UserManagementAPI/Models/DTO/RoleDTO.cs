namespace UserManagementAPI.Models.DTO
{
    public class RoleDTO
    {
        public Guid Id { get; set; }
        public required string RoleName { get; set; }
    }
}
