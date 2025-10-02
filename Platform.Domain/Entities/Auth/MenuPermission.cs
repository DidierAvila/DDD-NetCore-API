using System.ComponentModel.DataAnnotations.Schema;

namespace Platform.Domain.Entities.Auth
{
    [Table("MenuPermissions", Schema = "Auth")]
    public class MenuPermission
    {
        public Guid MenuId { get; set; }
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
