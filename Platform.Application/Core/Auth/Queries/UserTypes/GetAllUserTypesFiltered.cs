using AutoMapper;
using Platform.Domain.Repositories;
using Platform.Domain.DTOs.Auth;
using Platform.Domain.DTOs.Common;
using Platform.Domain.Entities.Auth;

namespace Platform.Application.Core.Auth.Queries.UserTypes
{
    public class GetAllUserTypesFiltered
    {
        private readonly IRepositoryBase<UserType> _userTypeRepository;

        public GetAllUserTypesFiltered(IRepositoryBase<UserType> userTypeRepository)
        {
            _userTypeRepository = userTypeRepository;
        }

        public async Task<PaginationResponseDto<UserTypeListResponseDto>> GetUserTypesFiltered(UserTypeFilterDto filter, CancellationToken cancellationToken)
        {

            // Validar y establecer valores por defecto
            if (filter.Page <= 0) filter.Page = 1;
            if (filter.PageSize <= 0) filter.PageSize = 10;
            if (filter.PageSize > 100) filter.PageSize = 100;

            // Obtener todos los tipos de usuario
            var allUserTypes = await _userTypeRepository.GetAll(cancellationToken);
            var query = allUserTypes.AsQueryable();

            // Aplicar filtros
            query = ApplyFilters(query, filter);

            // Contar total de registros
            var totalRecords = query.Count();

            // Aplicar ordenamiento
            query = ApplySorting(query, filter.SortBy);

            // Aplicar paginación
            var userTypes = query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            // Mapear a DTOs
            var userTypeDtos = userTypes.Select(ut => new UserTypeListResponseDto
            {
                Id = ut.Id,
                Name = ut.Name,
                Description = ut.Description,
                Status = ut.Status,
                UserCount = ut.Users?.Count ?? 0
            }).ToList();

            return userTypeDtos.ToPaginatedResult(
                filter.Page, 
                filter.PageSize, 
                totalRecords, 
                filter.SortBy);
        }

        private static IQueryable<UserType> ApplyFilters(IQueryable<UserType> query, UserTypeFilterDto filter)
        {
            // Filtro por nombre
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

        private IQueryable<UserType> ApplySorting(IQueryable<UserType> query, string? sortBy)
        {
            if (string.IsNullOrEmpty(sortBy))
            {
                // Ordenamiento por defecto
                return query.OrderBy(ut => ut.Name);
            }

            return sortBy.ToLower() switch
            {
                "name" => query.OrderBy(ut => ut.Name),
                "description" => query.OrderBy(ut => ut.Description),
                "status" => query.OrderBy(ut => ut.Status),
                _ => query.OrderBy(ut => ut.Name) // fallback al ordenamiento por defecto
            };
        }
    }
}
