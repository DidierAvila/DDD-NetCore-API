namespace Platform.Domain.DTOs.App
{
    public class ServiceDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool Status { get; set; }
        public decimal HourlyValue { get; set; }
        public Guid SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<ServiceCountryDto>? Countries { get; set; }
    }

    public class CreateServiceDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool Status { get; set; } = true;
        public decimal HourlyValue { get; set; }
        public Guid SupplierId { get; set; }
        public List<string>? CountryCodes { get; set; }
    }

    public class UpdateServiceDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool Status { get; set; }
        public decimal HourlyValue { get; set; }
        public Guid SupplierId { get; set; }
        public List<string>? CountryCodes { get; set; }
    }

    public class ServiceSummaryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool Status { get; set; }
        public decimal HourlyValue { get; set; }
        public Guid SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class ServiceWithDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool Status { get; set; }
        public decimal HourlyValue { get; set; }
        public Guid SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class ServiceDropdownDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
