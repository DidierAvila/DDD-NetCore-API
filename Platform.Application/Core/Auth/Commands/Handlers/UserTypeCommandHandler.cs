using Platform.Application.Core.Auth.Commands.UserTypes;
using Platform.Domain.DTOs.Auth;

namespace Platform.Application.Core.Auth.Commands.Handlers
{
    public class UserTypeCommandHandler : IUserTypeCommandHandler
    {
        private readonly CreateUserType _createUserType;
        private readonly UpdateUserType _updateUserType;
        private readonly DeleteUserType _deleteUserType;

        public UserTypeCommandHandler(
            CreateUserType createUserType,
            UpdateUserType updateUserType,
            DeleteUserType deleteUserType)
        {
            _createUserType = createUserType;
            _updateUserType = updateUserType;
            _deleteUserType = deleteUserType;
        }

        public async Task<UserTypeDto> CreateUserType(CreateUserTypeDto command, CancellationToken cancellationToken)
        {
            return await _createUserType.HandleAsync(command, cancellationToken);
        }

        public async Task<UserTypeDto> UpdateUserType(Guid id, UpdateUserTypeDto command, CancellationToken cancellationToken)
        {
            return await _updateUserType.HandleAsync(id, command, cancellationToken);
        }

        public async Task<bool> DeleteUserType(Guid id, CancellationToken cancellationToken)
        {
            return await _deleteUserType.HandleAsync(id, cancellationToken);
        }
    }
}
