using Microsoft.EntityFrameworkCore;

namespace Test_Backend.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Student> students { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            // User
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(u => u.id);

                entity.HasOne(u => u.Role)
                    .WithMany(r => r.users)
                    .HasForeignKey(u => u.RoleId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(u => u.Carts)
                    .WithOne(c => c.User)
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(u => u.Orders)
                    .WithOne(o => o.User)
                    .HasForeignKey(o => o.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
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
        }
    }
}
