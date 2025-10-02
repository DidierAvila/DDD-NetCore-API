using Platform.Domain.Repositories;
using Platform.Domain.DTOs.Auth;
using Platform.Domain.DTOs.Common;
using Platform.Domain.Entities.Auth;

namespace Platform.Application.Core.Auth.Queries.Roles
{
    public class GetAllRolesFiltered
    {
        private readonly IRepositoryBase<Role> _roleRepository;

        public GetAllRolesFiltered(IRepositoryBase<Role> roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<PaginationResponseDto<RoleListResponseDto>> GetRolesFiltered(RoleFilterDto filter, CancellationToken cancellationToken)
        {
            // Validar y establecer valores por defecto
            if (filter.Page <= 0) filter.Page = 1;
            if (filter.PageSize <= 0) filter.PageSize = 10;
            if (filter.PageSize > 100) filter.PageSize = 100;

            // Obtener todos los roles
            var allRoles = await _roleRepository.GetAll(cancellationToken);
            var query = allRoles.AsQueryable();

            // Aplicar filtros
            query = ApplyFilters(query, filter);

            // Contar total de registros
            var totalRecords = query.Count();

            // Aplicar ordenamiento
            query = ApplySorting(query, filter.SortBy);

            // Aplicar paginación
            var roles = query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            // Mapear a DTOs
            var roleDtos = roles.Select(r => new RoleListResponseDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                Status = r.Status,
                UserCount = r.Users?.Count ?? 0,
                PermissionCount = r.Permissions?.Count ?? 0,
                CreatedAt = r.CreatedAt
            }).ToList();

            return roleDtos.ToPaginatedResult(
                filter.Page, 
                filter.PageSize, 
                totalRecords, 
                filter.SortBy);
        }

        private static IQueryable<Role> ApplyFilters(IQueryable<Role> query, RoleFilterDto filter)
        {
            // Filtro por nombre
            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                query = query.Where(r => r.Name.ToLower().Contains(filter.Name.ToLower()));
            }

            // Filtro por estado
            if (filter.Status.HasValue)
            {
                query = query.Where(r => r.Status == filter.Status.Value);
            }

            return query;
        }

        private static IQueryable<Role> ApplySorting(IQueryable<Role> query, string? sortBy)
        {
            if (string.IsNullOrEmpty(sortBy))
            {
                // Ordenamiento por defecto
                return query.OrderBy(r => r.Name);
            }

            return sortBy.ToLower() switch
            {
                "name" => query.OrderBy(r => r.Name),
                "description" => query.OrderBy(r => r.Description),
                "status" => query.OrderBy(r => r.Status),
                "createdat" => query.OrderBy(r => r.CreatedAt),
                _ => query.OrderBy(r => r.Name) // fallback al ordenamiento por defecto
            };
        }
    }
}
