using UserManagementAPI.Models.Domain;

namespace UserManagementAPI.Models.DTO
{
    public class CreateUserRequestDTO
    {
        public required Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? Phone { get; set; }

        public required Guid RoleId { get; set; }
    }
}
