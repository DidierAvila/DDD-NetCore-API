using Microsoft.EntityFrameworkCore;
using Platform.Domain.Entities.Auth;
using Platform.Domain.Entities.App;

namespace Platform.Infrastructure.DbContexts
{
    public partial class PlatformDbContext : DbContext
    {
        public PlatformDbContext(DbContextOptions<PlatformDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> AuthAccounts { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<MenuPermission> MenuPermissions { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<UserType> UserTypes { get; set; }
        public virtual DbSet<UserTypePortalConfig> UserTypePortalConfigs { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<ServiceCountry> ServiceCountries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar claves primarias compuestas
            modelBuilder.Entity<MenuPermission>()
                .HasKey(mp => new { mp.MenuId, mp.PermissionId });

            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            // Configurar relaciones
            modelBuilder.Entity<MenuPermission>()
                .HasOne(mp => mp.Menu)
                .WithMany(m => m.MenuPermissions)
                .HasForeignKey(mp => mp.MenuId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MenuPermission>()
                .HasOne(mp => mp.Permission)
                .WithMany(p => p.MenuPermissions)
                .HasForeignKey(mp => mp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany()
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany()
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configurar otras relaciones
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserType)
                .WithMany(ut => ut.Users)
                .HasForeignKey(u => u.UserTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserTypePortalConfig>()
                .HasOne(utpc => utpc.UserType)
                .WithMany()
                .HasForeignKey(utpc => utpc.UserTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Account>()
                .HasOne(a => a.User)
                .WithMany(u => u.Accounts)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Session>()
                .HasOne(s => s.User)
                .WithMany(u => u.Sessions)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // Configurar relación ServiceCountry
            modelBuilder.Entity<ServiceCountry>()
                .HasOne(sc => sc.Service)
                .WithMany(s => s.ServiceCountries)
                .HasForeignKey(sc => sc.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);
                
            modelBuilder.Entity<ServiceCountry>()
                .HasOne(sc => sc.Country)
                .WithMany(c => c.ServiceCountries)
                .HasForeignKey(sc => sc.CountryCode)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
