using UserManagementAPI.Models.Domain;

namespace UserManagementAPI.Repositories.Interface
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllAsync();
        Task<Role?> GetByIdAsync(Guid id);
    }
}
