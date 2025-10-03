namespace Platform.Domain.DTOs.App
{
    public class CountryDto
    {
        public string Alpha2Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Alpha3Code { get; set; }
        public string? Capital { get; set; }
        public string? Region { get; set; }
    }
}