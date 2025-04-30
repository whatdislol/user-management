using System.Text.Json.Serialization;

namespace UserManagementAPI.Models.DTO
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string DateCreated { get; set; }
        public string? Phone { get; set; }
        public required RoleDTO Role { get; set; }
        public required List<PermissionDTO> Permissions { get; set; } = new List<PermissionDTO>();
    }
}
