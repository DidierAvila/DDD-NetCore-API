using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Platform.Domain.Entities.Auth
{
    [Table(name: "Permissions", Schema = "Auth")]
    public partial class Permission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public required Boolean Status { get; set; }

        public required DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = [];
        public virtual ICollection<MenuPermission> MenuPermissions { get; set; } = [];

        // Helper property para acceso directo a roles
        [NotMapped]
        public virtual ICollection<Role> Roles => [.. RolePermissions.Select(rp => rp.Role)];
        
        // Helper property para acceso directo a menus
        [NotMapped]
        public virtual ICollection<Menu> Menus => [.. MenuPermissions.Select(mp => mp.Menu)];
    }
}
