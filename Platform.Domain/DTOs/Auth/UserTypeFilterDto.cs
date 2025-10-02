using Platform.Domain.DTOs.Common;

namespace Platform.Domain.DTOs.Auth
{
    public class UserTypeFilterDto : PaginationRequestDto
    {
        /// <summary>
        /// Filtrar por nombre del tipo de usuario
        /// </summary>
        public string? Name { get; set; }
        
        /// <summary>
        /// Filtrar por estado del tipo de usuario (activo/inactivo)
        /// </summary>
        public bool? Status { get; set; }
    }

    public class UserTypeListResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool Status { get; set; }
        public int UserCount { get; set; } // Cantidad de usuarios con este tipo
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
