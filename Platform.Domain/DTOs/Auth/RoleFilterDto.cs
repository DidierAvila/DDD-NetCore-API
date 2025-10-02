using Platform.Domain.DTOs.Common;

namespace Platform.Domain.DTOs.Auth
{
    public class RoleFilterDto : PaginationRequestDto
    {
        /// <summary>
        /// Filtrar por nombre del rol
        /// </summary>
        public string? Name { get; set; }
        
        /// <summary>
        /// Filtrar por estado del rol (activo/inactivo)
        /// </summary>
        public bool? Status { get; set; }
    }

    public class RoleListResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool Status { get; set; }
        public int UserCount { get; set; } // Cantidad de usuarios con este rol
        public int PermissionCount { get; set; } // Cantidad de permisos asignados
        public DateTime CreatedAt { get; set; }
    }
}
