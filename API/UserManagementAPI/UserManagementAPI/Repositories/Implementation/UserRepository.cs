using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Data;
using UserManagementAPI.Models.Domain;
using UserManagementAPI.Models.DTO;
using UserManagementAPI.Repositories.Interface;

namespace UserManagementAPI.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<User> CreateAsync(User user)
        {
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            // load Role and Permissions into user
            await dbContext.Entry(user)
                .Reference(u => u.Role)
                .LoadAsync();

            if (user.Role != null)
            {
                await dbContext.Entry(user.Role)
                    .Collection(r => r.Permissions)
                    .LoadAsync();
            }
            // -----------------------------------

            return user;
        }

        public async Task<(IEnumerable<User>, int, int, int)> GetAllAsync(SearchOptionsDTO options)
        {
            IQueryable<User> query = dbContext.Users
                .Include(u => u.Role)
                    .ThenInclude(r => r.Permissions);

            if (!string.IsNullOrWhiteSpace(options.SearchQuery))
            {
                var q = options.SearchQuery.Trim().ToLower();
                query = query.Where(u =>
                    u.FirstName.ToLower().Contains(q) ||
                    u.LastName.ToLower().Contains(q) ||
                    u.Username.ToLower().Contains(q) ||
                    u.Email.ToLower().Contains(q));
            }

            string orderBy = options.OrderBy ?? "Id";
            bool desc = string.Equals(
                options.OrderDirection,
                "desc",
                StringComparison.OrdinalIgnoreCase
            );

            query = (orderBy.ToLower(), desc) switch
            {
                ("firstname", false) => query.OrderBy(u => u.FirstName),
                ("firstname", true) => query.OrderByDescending(u => u.FirstName),

                ("lastname", false) => query.OrderBy(u => u.LastName),
                ("lastname", true) => query.OrderByDescending(u => u.LastName),

                ("username", false) => query.OrderBy(u => u.Username),
                ("username", true) => query.OrderByDescending(u => u.Username),

                ("email", false) => query.OrderBy(u => u.Email),
                ("email", true) => query.OrderByDescending(u => u.Email),

                _ => query.OrderBy(u => u.FirstName)
            };

            int totalCount = await query.CountAsync();
            int pageSize = options.PageSize.GetValueOrDefault(10);
            int pageNumber = options.PageNumber.GetValueOrDefault(1);

            if (pageSize < 1) pageSize = 10;
            if (pageNumber < 1) pageNumber = 1;

            query = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            var users = await query.ToListAsync();

            return (users, totalCount, pageSize, pageNumber);
        }


        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await dbContext.Users
                .Include(u => u.Role)
                    .ThenInclude(r => r.Permissions)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> UpdateAsync(User user)
        {
            var existingUser = await dbContext.Users
                .Include(u => u.Role)
                    .ThenInclude(r => r.Permissions)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            if (existingUser == null)
            {
                return null;
            }

            // uniqueness constraints
            if (existingUser.Username != user.Username &&
                await dbContext.Users.AnyAsync(u => u.Username == user.Username && u.Id != user.Id))
            {
                throw new InvalidOperationException("Username already exists");
            }

            if (existingUser.Email != user.Email &&
                await dbContext.Users.AnyAsync(u => u.Email == user.Email && u.Id != user.Id))
            {
                throw new InvalidOperationException("Email already exists");
            }

            if (user.Phone != null && existingUser.Phone != user.Phone &&
                await dbContext.Users.AnyAsync(u => u.Phone == user.Phone && u.Id != user.Id))
            {
                throw new InvalidOperationException("Phone number already exists");
            }

            var oldRoleId = existingUser.RoleId;

            dbContext.Entry(existingUser).CurrentValues.SetValues(user);

            if (oldRoleId != user.RoleId)
            {
                // update backnav
                existingUser.Role = await dbContext.Roles
                    .Include(r => r.Permissions)
                    .FirstOrDefaultAsync(r => r.Id == user.RoleId);
            }

            await dbContext.SaveChangesAsync();

            if (existingUser.Role == null)
            {
                await dbContext.Entry(existingUser)
                    .Reference(u => u.Role)
                    .LoadAsync();

                if (existingUser.Role != null)
                {
                    await dbContext.Entry(existingUser.Role)
                        .Collection(r => r.Permissions)
                        .LoadAsync();
                }
            }

            return existingUser;
        }

        public async Task<User?> DeleteAsync(Guid id)
        {
            var existingUser = await dbContext.Users
                .Include(u => u.Role)
                    .ThenInclude(r => r.Permissions)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (existingUser == null)
            {
                return null;
            }

            dbContext.Users.Remove(existingUser);
            await dbContext.SaveChangesAsync();

            return existingUser;
        }
    }
}
