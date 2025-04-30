using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Models.Domain;

namespace UserManagementAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) 
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var roleId1 = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var roleId2 = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var roleId3 = Guid.Parse("33333333-3333-3333-3333-333333333333");

            var permissionId1 = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var permissionId2 = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var permissionId3 = Guid.Parse("33333333-3333-3333-3333-333333333333");

            // Seed permissions
            modelBuilder.Entity<Permission>().HasData(
                new Permission
                {
                    Id = permissionId1,
                    PermissionName = "Can Read"
                },
                new Permission
                {
                    Id = permissionId2,
                    PermissionName = "Can Write"
                },
                new Permission
                {
                    Id = permissionId3,
                    PermissionName = "Can Delete"
                }
            );

            // Seed roles
            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = roleId1,
                    RoleName = "Super Admin"
                },
                new Role
                {
                    Id = roleId2,
                    RoleName = "Admin"
                },
                new Role
                {
                    Id = roleId3,
                    RoleName = "Employee"
                }
            );

            // many to many
            modelBuilder.Entity<Role>()
                .HasMany(r => r.Permissions)
                .WithMany(p => p.Roles)
                .UsingEntity(j =>
                {
                    j.ToTable("RolePermissions");

                    j.HasData(
                        new { RolesId = roleId1, PermissionsId = permissionId1 },
                        new { RolesId = roleId1, PermissionsId = permissionId2 },
                        new { RolesId = roleId1, PermissionsId = permissionId3 },
                        new { RolesId = roleId2, PermissionsId = permissionId1 },
                        new { RolesId = roleId2, PermissionsId = permissionId2 },
                        new { RolesId = roleId3, PermissionsId = permissionId1 }
                    );
                });

            // unique phone numbers when not null
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Phone)
                .IsUnique()
                .HasFilter("[Phone] IS NOT NULL");
        }
    }
}
