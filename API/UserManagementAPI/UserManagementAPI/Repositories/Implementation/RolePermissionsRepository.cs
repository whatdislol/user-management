using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Data;
using UserManagementAPI.Models.Domain;
using UserManagementAPI.Repositories.Interface;

namespace UserManagementAPI.Repositories.Implementation
{
    public class RolePermissionsRepository : IRolePermissionsRepository
    {
        private readonly ApplicationDbContext dbContext;

        public RolePermissionsRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Permission>> GetAllPermissionsAsync()
        {
            return await dbContext.Permissions.ToListAsync();
        }

        public async Task<List<Permission>> GetPermissionsByRoleIdAsync(Guid roleId)
        {
            var role = await dbContext.Roles
                .Include(r => r.Permissions)
                .FirstOrDefaultAsync(r => r.Id == roleId);

            return role?.Permissions.ToList() ?? new List<Permission>();
        }

        public async Task<Role?> UpdateRolePermissionsAsync(Guid roleId, List<Guid> permissionIds)
        {
            var role = await dbContext.Roles
                .Include(r => r.Permissions)
                .FirstOrDefaultAsync(r => r.Id == roleId);

            if (role == null)
            {
                return null;
            }

            role.Permissions.Clear();

            // add selected permissions
            if (permissionIds.Any())
            {
                var permissions = await dbContext.Permissions
                    .Where(p => permissionIds.Contains(p.Id))
                    .ToListAsync();

                foreach (var permission in permissions)
                {
                    role.Permissions.Add(permission);
                }
            }

            await dbContext.SaveChangesAsync();

            return role;
        }
    }
}
