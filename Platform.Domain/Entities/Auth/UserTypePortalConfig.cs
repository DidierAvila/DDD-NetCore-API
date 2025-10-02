using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Platform.Domain.Entities.Auth
{
    [Table(name: "UserTypePortalConfigs", Schema = "Auth")]
    public class UserTypePortalConfig
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Guid UserTypeId { get; set; }

        public bool Status { get; set; } = true;

        public string Theme { get; set; } = "default";

        public string? DefaultLandingPage { get; set; }

        public string? LogoUrl { get; set; }

        public string Language { get; set; } = "es";

        [Column(TypeName = "text")]
        public string? AdditionalConfig { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        [ForeignKey("UserTypeId")]
        public virtual UserType? UserType { get; set; }
    }
}
