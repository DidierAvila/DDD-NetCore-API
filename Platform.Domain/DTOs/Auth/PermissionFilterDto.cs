using Platform.Domain.DTOs.Common;

namespace Platform.Domain.DTOs.Auth  
{
    public class PermissionFilterDto : PaginationRequestDto
    {
        /// <summary>
        /// Filtrar por nombre del permiso
        /// </summary>
        public string? Name { get; set; }
        
        /// <summary>
        /// Filtrar por estado del permiso (activo/inactivo)
        /// </summary>
        public bool? Status { get; set; }
    }

    public class PermissionListResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool Status { get; set; }
        public int RoleCount { get; set; } // Cantidad de roles que tienen este permiso
        public DateTime CreatedAt { get; set; }
    }
}
