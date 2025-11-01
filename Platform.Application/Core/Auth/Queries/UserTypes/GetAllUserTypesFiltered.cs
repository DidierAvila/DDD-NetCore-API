using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Platform.Application.Core.Common.Pagination;
using Platform.Domain.DTOs.Auth;
using Platform.Domain.DTOs.Common;
using Platform.Domain.Entities.Auth;
using Platform.Infrastructure.DbContexts;

namespace Platform.Application.Core.Auth.Queries.UserTypes
{
    public class GetAllUserTypesFiltered : PaginationServiceBase<UserType, UserTypeListResponseDto, UserTypeFilterDto>
    {
        private readonly PlatformDbContext _context;
        private readonly IMapper _mapper;

        public GetAllUserTypesFiltered(PlatformDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginationResponseDto<UserTypeListResponseDto>> GetUserTypesFiltered(UserTypeFilterDto filter, CancellationToken cancellationToken)
        {
            // Usar la query directa del contexto de EF para mantener IAsyncQueryProvider
            var baseQuery = _context.UserTypes
                .Include(ut => ut.Users) // Para UserCount si es necesario
                .AsQueryable();

            return await GetPaginatedAsync(baseQuery, filter, cancellationToken);
        }

        protected override IQueryable<UserType> ApplyFilters(IQueryable<UserType> query, UserTypeFilterDto filter)
        {
            // Búsqueda general en nombre y descripción
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var searchTerm = filter.Search.ToLower();
                query = query.Where(ut => ut.Name.ToLower().Contains(searchTerm) || 
                                         (ut.Description != null && ut.Description.ToLower().Contains(searchTerm)));
            }
            
            // Filtro por nombre específico
            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                query = query.Where(ut => ut.Name.ToLower().Contains(filter.Name.ToLower()));
            }

            // Filtro por estado
            if (filter.Status.HasValue)
            {
                query = query.Where(ut => ut.Status == filter.Status.Value);
            }

            return query;
        }

        protected override IQueryable<UserType> ApplySorting(IQueryable<UserType> query, string? sortBy, bool sortDescending)
        {
            return SortingHelper.CreateSortingBuilder(query)
                .AddSortMapping("name", ut => ut.Name)
                .AddSortMapping("description", ut => ut.Description ?? string.Empty)
                .AddSortMapping("status", ut => ut.Status)
                .SetDefaultSort(ut => ut.Name)
                .ApplySorting(sortBy, sortDescending);
        }

        protected override async Task<IEnumerable<UserTypeListResponseDto>> MapToDto(IEnumerable<UserType> entities, CancellationToken cancellationToken)
        {
            IEnumerable<UserTypeListResponseDto> userTypeDtos = _mapper.Map<IEnumerable<UserTypeListResponseDto>>(entities);
            return await Task.FromResult(userTypeDtos);
        }
    }
}
