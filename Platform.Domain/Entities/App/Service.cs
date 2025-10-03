using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Platform.Domain.Entities.Auth;

namespace Platform.Domain.Entities.App
{
    [Table("Services", Schema = "App")]
    public partial class Service
    {
        [Key]
        public Guid Id { get; set; }

        public Guid SupplierId { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public required bool Status { get; set; }

        public required decimal HourlyValue { get; set; }
        
        public required DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [NotMapped]
        public User? Supplier { get; set; }
        
        // Relación muchos a muchos con Country
        public virtual ICollection<ServiceCountry> ServiceCountries { get; set; } = [];
    }
}
