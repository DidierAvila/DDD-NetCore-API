using Platform.Domain.DTOs.Common;

namespace Platform.Domain.DTOs.App
{
    public class ServiceFilterDto : PaginationRequestDto
    {
        /// <summary>
        /// Filtrar por nombre del servicio
        /// </summary>
        public string? Name { get; set; }
        
        /// <summary>
        /// Filtrar por estado del servicio (activo/inactivo)
        /// </summary>
        public bool? Status { get; set; }
    }

    public class ServiceListResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}