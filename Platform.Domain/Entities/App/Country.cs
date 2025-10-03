using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Platform.Domain.Entities.App
{
    [Table("Countries", Schema = "App")]
    public class Country
    {
        [Key]
        [MaxLength(2)]
        public string Alpha2Code { get; set; } = null!;
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        
        [MaxLength(3)]
        public string? Alpha3Code { get; set; }
        
        [MaxLength(100)]
        public string? Capital { get; set; }
        
        [MaxLength(50)]
        public string? Region { get; set; }
        
        // Relación muchos a muchos con Service
        public virtual ICollection<ServiceCountry> ServiceCountries { get; set; } = new List<ServiceCountry>();
    }
}