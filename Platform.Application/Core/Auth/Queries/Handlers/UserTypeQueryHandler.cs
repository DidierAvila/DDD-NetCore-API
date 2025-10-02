using Platform.Application.Core.Auth.Queries.UserTypes;
using Platform.Domain.DTOs.Auth;
using Platform.Domain.DTOs.Common;

namespace Platform.Application.Core.Auth.Queries.Handlers
{
    public class UserTypeQueryHandler : IUserTypeQueryHandler
    {
        private readonly GetUserTypeById _getUserTypeById;
        private readonly GetAllUserTypes _getAllUserTypes;
        private readonly GetActiveUserTypes _getActiveUserTypes;
        private readonly GetUserTypesSummary _getUserTypesSummary;
        private readonly GetAllUserTypesFiltered _getAllUserTypesFiltered;
        private readonly GetUserTypesForDropdown _getUserTypesForDropdown;

        public UserTypeQueryHandler(
            GetUserTypeById getUserTypeById,
            GetAllUserTypes getAllUserTypes,
            GetActiveUserTypes getActiveUserTypes,
            GetUserTypesSummary getUserTypesSummary,
            GetAllUserTypesFiltered getAllUserTypesFiltered,
            GetUserTypesForDropdown getUserTypesForDropdown)
        {
            _getUserTypeById = getUserTypeById;
            _getAllUserTypes = getAllUserTypes;
            _getActiveUserTypes = getActiveUserTypes;
            _getUserTypesSummary = getUserTypesSummary;
            _getAllUserTypesFiltered = getAllUserTypesFiltered;
            _getUserTypesForDropdown = getUserTypesForDropdown;
        }

        public async Task<UserTypeDto?> GetUserTypeById(Guid id, CancellationToken cancellationToken)
        {
            return await _getUserTypeById.HandleAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<UserTypeDto>> GetAllUserTypes(CancellationToken cancellationToken)
        {
            return await _getAllUserTypes.HandleAsync(cancellationToken);
        }

        public async Task<IEnumerable<UserTypeDto>> GetActiveUserTypes(CancellationToken cancellationToken)
        {
            return await _getActiveUserTypes.HandleAsync(cancellationToken);
        }

        public async Task<IEnumerable<UserTypeSummaryDto>> GetUserTypesSummary(CancellationToken cancellationToken)
        {
            return await _getUserTypesSummary.HandleAsync(cancellationToken);
        }

        public async Task<PaginationResponseDto<UserTypeListResponseDto>> GetAllUserTypesFiltered(UserTypeFilterDto filter, CancellationToken cancellationToken)
        {
            return await _getAllUserTypesFiltered.GetUserTypesFiltered(filter, cancellationToken);
        }

        public async Task<IEnumerable<UserTypeDropdownDto>> GetUserTypesForDropdown(CancellationToken cancellationToken)
        {
            return await _getUserTypesForDropdown.HandleAsync(cancellationToken);
        }
    }
}
