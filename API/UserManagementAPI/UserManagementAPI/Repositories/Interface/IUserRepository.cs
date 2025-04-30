using UserManagementAPI.Models.Domain;
using UserManagementAPI.Models.DTO;

namespace UserManagementAPI.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User user);
        Task<(IEnumerable<User>, int, int, int)> GetAllAsync(SearchOptionsDTO options);
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> UpdateAsync(User user);
        Task<User?> DeleteAsync(Guid id);
    }
}
