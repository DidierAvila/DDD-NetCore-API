using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Platform.Domain.Entities.App
{
    [Table("Services", Schema = "App")]
    public partial class Service
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(200)]
        public required string Endpoint { get; set; }

        [Required]
        [MaxLength(10)]
        public required string Method { get; set; } // GET, POST, PUT, DELETE, etc.

        public required bool Status { get; set; }

        public required DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
