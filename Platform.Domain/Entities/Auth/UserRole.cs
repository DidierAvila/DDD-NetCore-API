using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Platform.Domain.Entities.Auth
{
    [Table("UserRoles", Schema = "Auth")]
    public class UserRole
    {
        [Key]
        [Column(Order = 0)]
        public Guid RoleId { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid UserId { get; set; }

        // Navigation properties
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; } = null!;

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }
}
