using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Platform.Domain.Entities.Auth
{
    [Table("MenuPermissions", Schema = "Auth")]
    public class MenuPermission
    {
        [Key]
        [Column(Order = 0)]
        public Guid MenuId { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid PermissionId { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        [ForeignKey("MenuId")]
        public virtual Menu? Menu { get; set; }

        [ForeignKey("PermissionId")]
        public virtual Permission? Permission { get; set; }
    }
}
