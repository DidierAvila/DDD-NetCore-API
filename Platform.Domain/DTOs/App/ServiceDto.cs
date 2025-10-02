using Platform.Domain.DTOs.Auth;

namespace Platform.Domain.DTOs.App
{
    public class ServiceDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string Endpoint { get; set; } = null!;
        public string Method { get; set; } = null!;
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateServiceDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string Endpoint { get; set; } = null!;
        public string Method { get; set; } = null!;
        public bool Status { get; set; } = true;
        public List<Guid>? PermissionIds { get; set; }
    }

    public class UpdateServiceDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string Endpoint { get; set; } = null!;
        public string Method { get; set; } = null!;
        public bool Status { get; set; }
        public List<Guid>? PermissionIds { get; set; }
    }

    public class ServiceSummaryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string Endpoint { get; set; } = null!;
        public string Method { get; set; } = null!;
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int PermissionCount { get; set; }
    }

    public class ServiceWithDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string Endpoint { get; set; } = null!;
        public string Method { get; set; } = null!;
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<PermissionDto> Permissions { get; set; } = new List<PermissionDto>();
    }

    /// <summary>
    /// DTO optimizado para dropdowns/listas desplegables de servicios (máximo rendimiento)
    /// </summary>
    public class ServiceDropdownDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }
}