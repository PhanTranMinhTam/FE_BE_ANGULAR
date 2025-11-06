using Microsoft.EntityFrameworkCore;

namespace Test_Backend.Data
{
    public class DbInitializer
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            // Seed Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { IdRoles = 1, Name = "Admin" },
                new Role { IdRoles = 2, Name = "User" }
            );
            // Seed Permissions
            modelBuilder.Entity<Permission>().HasData(
                new Permission { IdPermission = 1, Name = "CreateUser" },
                new Permission { IdPermission = 2, Name = "ViewUser" },
                new Permission { IdPermission = 3, Name = "UpdateteUser" },
                new Permission { IdPermission = 4, Name = "DeleteUser" }
            );

            // Seed RolePermissions
            modelBuilder.Entity<Rolepermission>().HasData(
                new Rolepermission { IdRole = 1, IdPermission = 1 },
                new Rolepermission { IdRole = 1, IdPermission = 2 },
                new Rolepermission { IdRole = 1, IdPermission = 3 },
                new Rolepermission { IdRole = 1, IdPermission = 4 },
                new Rolepermission { IdRole = 2, IdPermission = 2 }
            );

            // Hash the password before seeding
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword("Admin12345@");

            // Seed Users
            modelBuilder.Entity<User>().HasData(
                new User { IdUser = 1, Name = "admin", Email = "admin@example.com", PasswordHash = hashedPassword, PhoneNumber = "0345567810", RoleId = 1 }
            );
        }
    }
}
