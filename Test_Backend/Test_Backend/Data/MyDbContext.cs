using Microsoft.EntityFrameworkCore;

namespace Test_Backend.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Student> Students { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Rolepermission> Rolepermissions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Lop).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Diem).HasDefaultValue(0);
            });
            // User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.IdUser);

                entity.HasOne(u => u.Role)
                    .WithMany(r => r.users)
                    .HasForeignKey(u => u.RoleId)
                    .OnDelete(DeleteBehavior.Restrict);

            });


            // Role
            modelBuilder.Entity<Role>()
                .HasKey(r => r.IdRoles);
            modelBuilder.Entity<Role>()
                .HasMany(r => r.users)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Role>()
                .HasMany(r => r.RolePermissions)
                .WithOne(rp => rp.Role)
                .HasForeignKey(rp => rp.IdRole)
                .OnDelete(DeleteBehavior.Cascade);

            // Permission
            modelBuilder.Entity<Permission>()
                .HasKey(p => p.IdPermission);
            modelBuilder.Entity<Permission>()
                .HasMany(p => p.RolePermissions)
                .WithOne(rp => rp.Permission)
                .HasForeignKey(rp => rp.IdPermission)
                .OnDelete(DeleteBehavior.Cascade);

            // RolePermission
            modelBuilder.Entity<Rolepermission>()
                .HasKey(rp => new { rp.IdRole, rp.IdPermission });
            modelBuilder.Entity<Rolepermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.IdRole);
            modelBuilder.Entity<Rolepermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.IdPermission);
            DbInitializer.Seed(modelBuilder);
        }
    }
}
