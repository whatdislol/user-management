using UserManagementAPI.Models.Domain;

namespace UserManagementAPI.Repositories.Interface
{
    public interface IPermissionRepository
    {
        Task<IEnumerable<Permission>> GetAllAsync();
        Task<Permission?> GetByIdAsync(Guid id);
    }
}
