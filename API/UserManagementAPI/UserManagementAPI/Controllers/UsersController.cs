using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Data;
using UserManagementAPI.Models.Domain;
using UserManagementAPI.Models.DTO;
using UserManagementAPI.Repositories.Interface;

namespace UserManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IPasswordHasher<User> hasher;

        public UsersController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
            this.hasher = new PasswordHasher<User>();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserRequestDTO request)
        {
            // Map DTO to Domain Model
            var user = new User
            {
                Id = request.Id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Username = request.Username,
                Email = request.Email,
                Password = request.Password,
                DateCreated = DateTime.UtcNow,
                Phone = request.Phone,
                RoleId = request.RoleId
            };
            user.Password = this.hasher.HashPassword(user, request.Password);

            var createdUser = await userRepository.CreateAsync(user);

            // Map Domain Model to DTO
            var response = new UserDTO
            {
                Id = createdUser.Id,
                FirstName = createdUser.FirstName,
                LastName = createdUser.LastName,
                Username = createdUser.Username,
                Email = createdUser.Email,
                DateCreated = createdUser.DateCreated.ToString("dd MMM, yyyy", CultureInfo.InvariantCulture),
                Phone = createdUser.Phone,
                Role = new RoleDTO
                {
                    Id = createdUser.Role.Id,
                    RoleName = createdUser.Role.RoleName,
                },
                Permissions = createdUser.Role.Permissions
                    .Select(p => new PermissionDTO
                    {
                        Id = p.Id,
                        PermissionName = p.PermissionName
                    })
                    .ToList()
            };

            return Ok(response);
        }

        // GET: /api/users
        [HttpGet]
        public async Task<IActionResult> GetAllUsers(
            [FromQuery] string? orderBy,
            [FromQuery] string? orderDirection,
            [FromQuery] int? pageNumber,
            [FromQuery] int? pageSize,
            [FromQuery] string? searchQuery
        )
        {
            var options = new SearchOptionsDTO
            {
                OrderBy = orderBy,
                OrderDirection = orderDirection,
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchQuery = searchQuery

            };
            var (users, totalCount, pSize, pNumber) = await userRepository.GetAllAsync(options);

            // Map Domain Model to DTO
            var data = new List<UserDTO>();
            foreach (var user in users)
            {
                data.Add(new UserDTO
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Username = user.Username,
                    Email = user.Email,
                    DateCreated = user.DateCreated.ToString("dd MMM, yyyy", CultureInfo.InvariantCulture),
                    Phone = user.Phone,
                    Role = new RoleDTO
                    {
                        Id = user.Role.Id,
                        RoleName = user.Role.RoleName,
                    },
                    Permissions = user.Role.Permissions
                    .Select(p => new PermissionDTO
                    {
                        Id = p.Id,
                        PermissionName = p.PermissionName
                    })
                    .ToList()
                });
            }

            var response = new AllUsersResponseDTO
            {
                Users = data,
                TotalCount = totalCount,
                PageSize = pSize,
                PageNumber = pNumber
            };

            return Ok(response);
        }

        // GET: /api/users/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetUserById([FromRoute] Guid id)
        {
            var existingUser = await userRepository.GetByIdAsync(id);

            if (existingUser == null)
            {
                return NotFound();
            }

            var response = new UserDTO
            {
                Id = existingUser.Id,
                FirstName = existingUser.FirstName,
                LastName = existingUser.LastName,
                Username = existingUser.Username,
                Email = existingUser.Email,
                DateCreated = existingUser.DateCreated.ToString("dd MMM, yyyy", CultureInfo.InvariantCulture),
                Phone = existingUser.Phone,
                Role = new RoleDTO
                {
                    Id = existingUser.Role.Id,
                    RoleName = existingUser.Role.RoleName,
                },
                Permissions = existingUser.Role.Permissions
                    .Select(p => new PermissionDTO
                    {
                        Id = p.Id,
                        PermissionName = p.PermissionName
                    })
                    .ToList()
            };

            return Ok(response);
        }

        // PUT: /api/users/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> EditUser([FromRoute] Guid id, UpdateUserRequestDTO request)
        {
            // DTO to Domain Model
            var existingUser = await userRepository.GetByIdAsync(id);
            var user = new User
            {
                Id = id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Username = request.Username,
                Email = request.Email,
                Password = request.Password,
                DateCreated = existingUser.DateCreated,
                Phone = request.Phone,
                RoleId = request.RoleId
            };

            user = await userRepository.UpdateAsync(user);

            if (user == null)
            {
                return NotFound();
            }

            // Domain Model to DTO
            var response = new UserDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Email = user.Email,
                DateCreated = user.DateCreated.ToString("dd MMM, yyyy", CultureInfo.InvariantCulture),
                Phone = user.Phone,
                Role = new RoleDTO
                {
                    Id = user.Role.Id,
                    RoleName = user.Role.RoleName,
                },
                Permissions = user.Role.Permissions
                    .Select(p => new PermissionDTO
                    {
                        Id = p.Id,
                        PermissionName = p.PermissionName
                    })
                    .ToList()
            };

            return Ok(response);
        }

        // DELETE: /api/users/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            var user = await userRepository.DeleteAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            // Domain Model to DTO
            var response = new UserDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Email = user.Email,
                DateCreated = user.DateCreated.ToString("dd MMM, yyyy", CultureInfo.InvariantCulture),
                Phone = user.Phone,
                Role = new RoleDTO
                {
                    Id = user.Role.Id,
                    RoleName = user.Role.RoleName,
                },
                Permissions = user.Role.Permissions
                    .Select(p => new PermissionDTO
                    {
                        Id = p.Id,
                        PermissionName = p.PermissionName
                    })
                    .ToList()
            };

            return Ok(response);
        }
    }
}
