using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Platform.Domain.Entities.Auth
{
    [Table("Menus", Schema = "Auth")]
    public class Menu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public required string Label { get; set; }
        public required string Icon { get; set; }
        public required string Route { get; set; }
        public required int Order { get; set; }
        public required Boolean IsGroup { get; set; }
        public Guid? ParentId { get; set; }
        public required Boolean Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<MenuPermission> MenuPermissions { get; set; } = [];
    }
}
