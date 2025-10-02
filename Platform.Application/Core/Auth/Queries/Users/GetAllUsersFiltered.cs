using AutoMapper;
using Platform.Domain.Repositories;
using Platform.Domain.DTOs.Auth;
using Platform.Domain.DTOs.Common;
using Platform.Domain.Entities.Auth;

namespace Platform.Application.Core.Auth.Queries.Users
{
    public class GetAllUsersFiltered
    {
        private readonly IRepositoryBase<User> _userRepository;
        private readonly IRepositoryBase<UserType> _userTypeRepository;
        private readonly IRepositoryBase<Role> _roleRepository;
        private readonly IRepositoryBase<UserRole> _userRoleRepository;
        private readonly IMapper _mapper;

        public GetAllUsersFiltered(IRepositoryBase<User> userRepository, IRepositoryBase<UserType> userTypeRepository, IRepositoryBase<Role> roleRepository, IRepositoryBase<UserRole> userRoleRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _userTypeRepository = userTypeRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
        }

        public async Task<PaginationResponseDto<UserBasicDto>> HandleAsync(UserFilterDto filter, CancellationToken cancellationToken)
        {
            // Validar parámetros de paginación
            if (filter.Page <= 0) filter.Page = 1;
            if (filter.PageSize <= 0) filter.PageSize = 10;
            if (filter.PageSize > 100) filter.PageSize = 100;

            // Obtener datos por separado para evitar problemas de navegación
            var allUsers = await _userRepository.GetAll(cancellationToken);
            var userTypes = await _userTypeRepository.GetAll(cancellationToken);
            var roles = await _roleRepository.GetAll(cancellationToken);
            var userRoles = await _userRoleRepository.GetAll(cancellationToken);

            var query = allUsers.AsQueryable();

            // Aplicar filtros
            query = ApplyFilters(query, filter);

            // Contar total de registros
            var totalRecords = query.Count();

            // Aplicar ordenamiento
            query = ApplySorting(query, filter.SortBy);

            // Aplicar paginación
            var users = query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            // Mapear a DTOs con datos cargados por separado
            var userDtos = users.Select(u => {
                var userType = userTypes.FirstOrDefault(ut => ut.Id == u.UserTypeId);
                
                // Buscar el primer rol del usuario
                var firstUserRole = userRoles.FirstOrDefault(ur => ur.UserId == u.Id);
                var firstRole = firstUserRole != null ? roles.FirstOrDefault(r => r.Id == firstUserRole.RoleId) : null;
                
                return new UserBasicDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Image = u.Image,
                    Status = u.Status,
                    UserTypeId = u.UserTypeId,
                    UserTypeName = userType?.Name,
                    FirstRoleName = firstRole?.Name,
                    CreatedAt = u.CreatedAt ?? DateTime.MinValue, // Manejo de nullable
                    UpdatedAt = u.UpdatedAt
                };
            }).ToList();

            return userDtos.ToPaginatedResult(
                filter.Page, 
                filter.PageSize, 
                totalRecords, 
                filter.SortBy);
        }

        private IQueryable<User> ApplyFilters(IQueryable<User> query, UserFilterDto filter)
        {
            // Filtro por búsqueda general (solo Name y Email ya que Username no existe)
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var searchTerm = filter.Search.ToLower();
                query = query.Where(u => 
                    u.Name.ToLower().Contains(searchTerm) ||
                    u.Email.ToLower().Contains(searchTerm));
            }

            // Filtro por nombre
            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                query = query.Where(u => u.Name.ToLower().Contains(filter.Name.ToLower()));
            }

            // Filtro por email
            if (!string.IsNullOrWhiteSpace(filter.Email))
            {
                query = query.Where(u => u.Email.ToLower().Contains(filter.Email.ToLower()));
            }

            // Filtro por rol - usando la colección Roles
            if (filter.RoleId.HasValue)
            {
                query = query.Where(u => u.Roles.Any(r => r.Id == filter.RoleId.Value));
            }

            // Filtro por tipo de usuario
            if (filter.UserTypeId.HasValue)
            {
                query = query.Where(u => u.UserTypeId == filter.UserTypeId.Value);
            }

            // Filtro por estado - omitido ya que no existe en la entidad actual
            // if (filter.Status.HasValue)
            // {
            //     query = query.Where(u => u.Status == filter.Status.Value);
            // }

            // Filtro por fecha de creación
            if (filter.CreatedAfter.HasValue && filter.CreatedAfter.HasValue)
            {
                query = query.Where(u => u.CreatedAt >= filter.CreatedAfter.Value);
            }

            if (filter.CreatedBefore.HasValue && filter.CreatedBefore.HasValue)
            {
                query = query.Where(u => u.CreatedAt <= filter.CreatedBefore.Value);
            }

            return query;
        }

        private IQueryable<User> ApplySorting(IQueryable<User> query, string? sortBy)
        {
            if (string.IsNullOrEmpty(sortBy))
            {
                // Ordenamiento por defecto
                return query.OrderBy(u => u.Name);
            }

            return sortBy.ToLower() switch
            {
                "name" => query.OrderBy(u => u.Name),
                "email" => query.OrderBy(u => u.Email),
                "username" => query.OrderBy(u => u.Email), // Usando email como username
                "createdat" => query.OrderBy(u => u.CreatedAt),
                "usertypeid" => query.OrderBy(u => u.UserTypeId),
                _ => query.OrderBy(u => u.Name) // fallback al ordenamiento por defecto
            };
        }
    }
}
