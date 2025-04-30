using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Models.DTO;
using UserManagementAPI.Repositories.Interface;

namespace UserManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolePermissionsController : ControllerBase
    {
        private readonly IRolePermissionsRepository rolePermissionRepository;
        private readonly IRoleRepository roleRepository;

        public RolePermissionsController(
            IRolePermissionsRepository rolePermissionRepository,
            IRoleRepository roleRepository)
        {
            this.rolePermissionRepository = rolePermissionRepository;
            this.roleRepository = roleRepository;
        }

        // GET: api/RolePermissions/{roleId}
        [HttpGet("{roleId:Guid}")]
        public async Task<IActionResult> GetRolePermissions([FromRoute] Guid roleId)
        {
            var role = await roleRepository.GetByIdAsync(roleId);
            if (role == null)
            {
                return NotFound();
            }

            var allPermissions = await rolePermissionRepository.GetAllPermissionsAsync();
            var rolePermissions = await rolePermissionRepository.GetPermissionsByRoleIdAsync(roleId);
            var rolePermissionIds = rolePermissions.Select(p => p.Id).ToList();

            var response = new RolePermissionsDTO
            {
                RoleId = role.Id,
                RoleName = role.RoleName,
                Permissions = allPermissions.Select(p => new PermissionCheckboxDTO
                {
                    Id = p.Id,
                    PermissionName = p.PermissionName,
                    IsSelected = rolePermissionIds.Contains(p.Id)
                }).ToList()
            };

            return Ok(response);
        }

        // PUT: api/RolePermissions
        [HttpPut]
        public async Task<IActionResult> UpdateRolePermissions(UpdateRolePermissionsRequestDTO request)
        {
            var updatedRole = await rolePermissionRepository.UpdateRolePermissionsAsync(request.RoleId, request.PermissionIds);

            if (updatedRole == null)
            {
                return NotFound();
            }

            // Domain Model to DTO
            var response = new UpdateRolePermissionsResponseDTO
            {
                RoleId = updatedRole.Id,
                RoleName = updatedRole.RoleName,
                Permissions = updatedRole.Permissions.Select(p => new PermissionDTO
                {
                    Id = p.Id,
                    PermissionName = p.PermissionName
                }).ToList()
            };

            return Ok(response);
        }
    }
}
