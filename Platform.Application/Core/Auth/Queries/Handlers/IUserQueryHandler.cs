using Platform.Domain.DTOs.Auth;
using Platform.Domain.DTOs.Common;

namespace Platform.Application.Core.Auth.Queries.Handlers
{
    public interface IUserQueryHandler
    {
        Task<UserDto?> GetUserById(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<UserDto>> GetAllUsers(CancellationToken cancellationToken);
        Task<IEnumerable<UserBasicDto>> GetAllUsersBasic(CancellationToken cancellationToken);
        Task<PaginationResponseDto<UserBasicDto>> GetAllUsersFiltered(UserFilterDto filter, CancellationToken cancellationToken);
        Task<IEnumerable<UserDropdownDto>> GetUsersByTypeForDropdown(Guid userTypeId, CancellationToken cancellationToken);
        Task<IEnumerable<UserDropdownDto>> GetSupplierUsersForDropdown(CancellationToken cancellationToken);
    }
}
