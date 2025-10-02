using AutoMapper;
using Platform.Domain.Repositories;
using Platform.Domain.DTOs.Auth;
using Platform.Domain.DTOs.Common;

namespace Platform.Application.Core.Auth.Queries.Permissions
{
    public class GetAllPermissionsFiltered
    {
        private readonly IRepositoryBase<Domain.Entities.Auth.Permission> _permissionRepository;
        private readonly IMapper _mapper;

        public GetAllPermissionsFiltered(IRepositoryBase<Domain.Entities.Auth.Permission> permissionRepository, IMapper mapper)
        {
            _permissionRepository = permissionRepository;
            _mapper = mapper;
        }

        public async Task<PaginationResponseDto<PermissionListResponseDto>> GetPermissionsFiltered(PermissionFilterDto filter, CancellationToken cancellationToken)
        {
            // Validar y establecer valores por defecto
            if (filter.Page <= 0) filter.Page = 1;
            if (filter.PageSize <= 0) filter.PageSize = 10;
            if (filter.PageSize > 100) filter.PageSize = 100;

            // Obtener todos los permisos
            var allPermissions = await _permissionRepository.GetAll(cancellationToken);
            var query = allPermissions.AsQueryable();

            // Aplicar filtros
            query = ApplyFilters(query, filter);

            // Contar total de registros
            var totalRecords = query.Count();

            // Aplicar ordenamiento
            query = ApplySorting(query, filter.SortBy);

            // Aplicar paginación
            var permissions = query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            // Mapear a DTOs
            var permissionDtos = permissions.Select(p => new PermissionListResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Status = p.Status,
                RoleCount = p.Roles?.Count ?? 0,
                CreatedAt = p.CreatedAt
            }).ToList();

            return permissionDtos.ToPaginatedResult(
                filter.Page, 
                filter.PageSize, 
                totalRecords, 
                filter.SortBy);
        }

        private IQueryable<Domain.Entities.Auth.Permission> ApplyFilters(IQueryable<Domain.Entities.Auth.Permission> query, PermissionFilterDto filter)
        {
            // Filtro por nombre
            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                query = query.Where(p => p.Name.ToLower().Contains(filter.Name.ToLower()));
            }

            // Filtro por estado
            if (filter.Status.HasValue)
            {
                query = query.Where(p => p.Status == filter.Status.Value);
            }

            return query;
        }

        private IQueryable<Domain.Entities.Auth.Permission> ApplySorting(IQueryable<Domain.Entities.Auth.Permission> query, string? sortBy)
        {
            if (string.IsNullOrEmpty(sortBy))
            {
                // Ordenamiento por defecto
                return query.OrderBy(p => p.Name);
            }

            return sortBy.ToLower() switch
            {
                "name" => query.OrderBy(p => p.Name),
                "description" => query.OrderBy(p => p.Description),
                "status" => query.OrderBy(p => p.Status),
                "createdat" => query.OrderBy(p => p.CreatedAt),
                _ => query.OrderBy(p => p.Name) // fallback al ordenamiento por defecto
            };
        }
    }
}
