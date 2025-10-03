using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Platform.Domain.Entities.App
{
    [Table("ServiceCountries", Schema = "App")]
    public class ServiceCountry
    {
        [Key]
        public int Id { get; set; }
        
        public Guid ServiceId { get; set; }
        
        [MaxLength(2)]
        public string CountryCode { get; set; } = null!;
        
        // Propiedades de navegación
        [ForeignKey("ServiceId")]
        public virtual Service Service { get; set; } = null!;
        
        [ForeignKey("CountryCode")]
        public virtual Country Country { get; set; } = null!;
    }
}