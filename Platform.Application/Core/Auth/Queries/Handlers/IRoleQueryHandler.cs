using Platform.Domain.DTOs.Auth;
using Platform.Domain.DTOs.Common;

namespace Platform.Application.Core.Auth.Queries.Handlers
{
    public interface IRoleQueryHandler
    {
        Task<RoleDto?> GetRoleById(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<RoleDto>> GetAllRoles(CancellationToken cancellationToken);
        Task<PaginationResponseDto<RoleListResponseDto>> GetAllRolesFiltered(RoleFilterDto filter, CancellationToken cancellationToken);
        Task<IEnumerable<RoleDropdownDto>> GetRolesDropdown(CancellationToken cancellationToken);
    }
}
