namespace Platform.Domain.DTOs.App
{
    public class ServiceCountryDto
    {
        public Guid ServiceId { get; set; }
        public string CountryCode { get; set; } = null!;
        public string CountryName { get; set; } = null!;
    }
    
    public class AssignCountryToServiceDto
    {
        public Guid ServiceId { get; set; }
        public List<string> CountryCodes { get; set; } = [];
    }
}