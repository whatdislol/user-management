using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Models.DTO;
using UserManagementAPI.Repositories.Implementation;
using UserManagementAPI.Repositories.Interface;

namespace UserManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionRepository permissionRepository;

        public PermissionsController(IPermissionRepository permissionRepository)
        {
            this.permissionRepository = permissionRepository;
        }

        // GET: /api/permissions
        [HttpGet]
        public async Task<IActionResult> GetAllPermissions()
        {
            var permissions = await permissionRepository.GetAllAsync();

            var response = new List<PermissionDTO>();
            foreach (var permission in permissions)
            {
                response.Add(new PermissionDTO
                {
                    Id = permission.Id,
                    PermissionName = permission.PermissionName
                });
            }

            return Ok(response);
        }

        // GET: /api/permissions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetPermissionById([FromRoute] Guid id)
        {
            var existingPermission = await permissionRepository.GetByIdAsync(id);

            if (existingPermission == null)
            {
                return NotFound();
            }

            var response = new PermissionDTO
            {
                Id = existingPermission.Id,
                PermissionName = existingPermission.PermissionName
            };

            return Ok(response);
        }
    }
}
