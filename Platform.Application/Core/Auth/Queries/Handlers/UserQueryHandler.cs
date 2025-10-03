using Platform.Application.Core.Auth.Queries.Users;
using Platform.Domain.DTOs.Auth;
using Platform.Domain.DTOs.Common;

namespace Platform.Application.Core.Auth.Queries.Handlers
{
    public class UserQueryHandler : IUserQueryHandler
    {
        private readonly GetUserById _getUserById;
        private readonly GetAllUsers _getAllUsers;
        private readonly GetAllUsersBasic _getAllUsersBasic;
        private readonly GetAllUsersFiltered _getAllUsersFiltered;
        private readonly GetUsersByTypeForDropdown _getUsersByTypeForDropdown;
        private readonly GetSupplierUsersForDropdown _getSupplierUsersForDropdown;

        public UserQueryHandler(
            GetUserById getUserById, 
            GetAllUsers getAllUsers,
            GetAllUsersBasic getAllUsersBasic,
            GetAllUsersFiltered getAllUsersFiltered,
            GetUsersByTypeForDropdown getUsersByTypeForDropdown,
            GetSupplierUsersForDropdown getSupplierUsersForDropdown)
        {
            _getUserById = getUserById;
            _getAllUsers = getAllUsers;
            _getAllUsersBasic = getAllUsersBasic;
            _getAllUsersFiltered = getAllUsersFiltered;
            _getUsersByTypeForDropdown = getUsersByTypeForDropdown;
            _getSupplierUsersForDropdown = getSupplierUsersForDropdown;
        }

        public async Task<UserDto?> GetUserById(Guid id, CancellationToken cancellationToken)
        {
            return await _getUserById.HandleAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsers(CancellationToken cancellationToken)
        {
            return await _getAllUsers.HandleAsync(cancellationToken);
        }

        public async Task<IEnumerable<UserBasicDto>> GetAllUsersBasic(CancellationToken cancellationToken)
        {
            return await _getAllUsersBasic.HandleAsync(cancellationToken);
        }

        public async Task<PaginationResponseDto<UserBasicDto>> GetAllUsersFiltered(UserFilterDto filter, CancellationToken cancellationToken)
        {
            return await _getAllUsersFiltered.HandleAsync(filter, cancellationToken);
        }

        public async Task<IEnumerable<UserDropdownDto>> GetUsersByTypeForDropdown(Guid userTypeId, CancellationToken cancellationToken)
        {
            return await _getUsersByTypeForDropdown.HandleAsync(userTypeId, cancellationToken);
        }

        public async Task<IEnumerable<UserDropdownDto>> GetSupplierUsersForDropdown(CancellationToken cancellationToken)
        {
            return await _getSupplierUsersForDropdown.HandleAsync(cancellationToken);
        }
    }
}
