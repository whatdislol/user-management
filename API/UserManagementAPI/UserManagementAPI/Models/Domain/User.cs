using Microsoft.EntityFrameworkCore;

namespace UserManagementAPI.Models.Domain
{
    [Index(nameof(Username), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(Phone), IsUnique = true)]
    public class User
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required DateTime DateCreated { get; set; }
        public string? Phone { get; set; }

        public required Guid RoleId { get; set; }
        public Role? Role { get; set; }
    }
}
