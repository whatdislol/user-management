using UserManagementAPI.Models.Domain;

namespace UserManagementAPI.Repositories.Interface
{
    public interface IRolePermissionsRepository
    {
        Task<List<Permission>> GetAllPermissionsAsync();
        Task<List<Permission>> GetPermissionsByRoleIdAsync(Guid roleId);
        Task<Role?> UpdateRolePermissionsAsync(Guid roleId, List<Guid> permissionIds);
    }
}
