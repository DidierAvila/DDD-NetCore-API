using Platform.Domain.DTOs.Auth;
using Platform.Domain.DTOs.Common;

namespace Platform.Application.Core.Auth.Queries.Handlers
{
    public interface IUserTypeQueryHandler
    {
        Task<UserTypeDto?> GetUserTypeById(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<UserTypeDto>> GetAllUserTypes(CancellationToken cancellationToken);
        Task<IEnumerable<UserTypeDto>> GetActiveUserTypes(CancellationToken cancellationToken);
        Task<IEnumerable<UserTypeSummaryDto>> GetUserTypesSummary(CancellationToken cancellationToken);
        Task<PaginationResponseDto<UserTypeListResponseDto>> GetAllUserTypesFiltered(UserTypeFilterDto filter, CancellationToken cancellationToken);
        Task<IEnumerable<UserTypeDropdownDto>> GetUserTypesForDropdown(CancellationToken cancellationToken);
    }
}
