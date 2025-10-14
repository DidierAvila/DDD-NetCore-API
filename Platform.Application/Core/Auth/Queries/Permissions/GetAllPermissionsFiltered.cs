using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Platform.Application.Core.Common.Pagination;
using Platform.Domain.DTOs.Auth;
using Platform.Domain.DTOs.Common;
using Platform.Infrastructure.DbContexts;

namespace Platform.Application.Core.Auth.Queries.Permissions
{
    public class GetAllPermissionsFiltered : PaginationServiceBase<Domain.Entities.Auth.Permission, PermissionListResponseDto, PermissionFilterDto>
    {
        private readonly PlatformDbContext _context;
        private readonly IMapper _mapper;

        public GetAllPermissionsFiltered(PlatformDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginationResponseDto<PermissionListResponseDto>> GetPermissionsFiltered(PermissionFilterDto filter, CancellationToken cancellationToken)
        {
            // Usar la query directa del contexto de EF para mantener IAsyncQueryProvider
            var baseQuery = _context.Permissions
                .Include(p => p.RolePermissions) // Incluir la relación real
                .ThenInclude(rp => rp.Role) // Luego incluir Role
                .AsQueryable();

            return await GetPaginatedAsync(baseQuery, filter, cancellationToken);
        }

        protected override IQueryable<Domain.Entities.Auth.Permission> ApplyFilters(IQueryable<Domain.Entities.Auth.Permission> query, PermissionFilterDto filter)
        {
            // Búsqueda general en nombre y descripción
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var searchTerm = filter.Search.ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(searchTerm) || 
                                        (p.Description != null && p.Description.ToLower().Contains(searchTerm)));
            }
            
            // Filtro por nombre específico
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

        protected override IQueryable<Domain.Entities.Auth.Permission> ApplySorting(IQueryable<Domain.Entities.Auth.Permission> query, string? sortBy, bool sortDescending)
        {
            return SortingHelper.CreateSortingBuilder(query)
                .AddSortMapping("name", p => p.Name)
                .AddSortMapping("description", p => p.Description ?? string.Empty)
                .AddSortMapping("status", p => p.Status)
                .AddSortMapping("createdat", p => p.CreatedAt)
                .SetDefaultSort(p => p.Name)
                .ApplySorting(sortBy, sortDescending);
        }

        protected override async Task<IEnumerable<PermissionListResponseDto>> MapToDto(IEnumerable<Domain.Entities.Auth.Permission> entities, CancellationToken cancellationToken)
        {
            var permissionDtos = entities.Select(p => new PermissionListResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Status = p.Status,
                RoleCount = p.RolePermissions?.Count ?? 0, // Usar la relación real
                CreatedAt = p.CreatedAt
            }).ToList();

            return await Task.FromResult(permissionDtos);
        }
    }
}
