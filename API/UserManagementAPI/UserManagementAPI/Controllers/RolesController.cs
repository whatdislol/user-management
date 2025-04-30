using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Models.DTO;
using UserManagementAPI.Repositories.Implementation;
using UserManagementAPI.Repositories.Interface;

namespace UserManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepository roleRepository;

        public RolesController(IRoleRepository roleRepository)
        {
            this.roleRepository = roleRepository;
        }

        // GET: /api/roles
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await roleRepository.GetAllAsync();

            var response = new List<RoleDTO>();
            foreach (var role in roles)
            {
                response.Add(new RoleDTO 
                { 
                    Id = role.Id,
                    RoleName = role.RoleName
                });
            }

            return Ok(response);
        }

        // GET: /api/roles/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetRoleById([FromRoute] Guid id)
        {
            var existingRole = await roleRepository.GetByIdAsync(id);

            if (existingRole == null)
            {
                return NotFound();
            }

            var response = new RoleDTO
            {
                Id = existingRole.Id,
                RoleName = existingRole.RoleName
            };

            return Ok(response);
        }
    }
}
