using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Data;
using UserManagementAPI.Models.Domain;
using UserManagementAPI.Repositories.Interface;

namespace UserManagementAPI.Repositories.Implementation
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly ApplicationDbContext dbContext;

        public PermissionRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Permission>> GetAllAsync()
        {
            return await dbContext.Permissions.ToListAsync();
        }

        public async Task<Permission?> GetByIdAsync(Guid id)
        {
            return await dbContext.Permissions.FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
