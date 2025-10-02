using Platform.Domain.DTOs.Auth;
using Platform.Domain.DTOs.Common;

namespace Platform.Application.Core.Auth.Queries.Handlers
{
    public interface IPermissionQueryHandler
    {
        Task<PermissionDto?> GetPermissionById(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<PermissionDto>> GetAllPermissions(CancellationToken cancellationToken);
        Task<IEnumerable<PermissionDto>> GetActivePermissions(CancellationToken cancellationToken);
        Task<IEnumerable<PermissionSummaryDto>> GetPermissionsSummary(CancellationToken cancellationToken);
        Task<PaginationResponseDto<PermissionListResponseDto>> GetAllPermissionsFiltered(PermissionFilterDto filter, CancellationToken cancellationToken);
        Task<IEnumerable<PermissionDropdownDto>> GetPermissionsForDropdown(CancellationToken cancellationToken);
    }
}
