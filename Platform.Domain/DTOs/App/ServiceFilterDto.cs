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
        /// Filtrar por endpoint del servicio
        /// </summary>
        public string? Endpoint { get; set; }
        
        /// <summary>
        /// Filtrar por método HTTP del servicio
        /// </summary>
        public string? Method { get; set; }
        
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
        public string Endpoint { get; set; } = null!;
        public string Method { get; set; } = null!;
        public bool Status { get; set; }
        public int PermissionCount { get; set; } // Cantidad de permisos asignados
        public DateTime CreatedAt { get; set; }
    }
}