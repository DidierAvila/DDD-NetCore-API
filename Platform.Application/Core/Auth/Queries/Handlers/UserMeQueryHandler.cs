using Platform.Application.Core.Auth.Queries.UserMe;
using Platform.Domain.DTOs.Auth;

namespace Platform.Application.Core.Auth.Queries.Handlers
{
    public class UserMeQueryHandler : IUserMeQueryHandler
    {
        private readonly GetUserMe _getUserMeQuery;

        public UserMeQueryHandler(GetUserMe getUserMeQuery)
        {
            _getUserMeQuery = getUserMeQuery;
        }

        public async Task<UserMeResponseDto> GetUserMe(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _getUserMeQuery.ExecuteAsync(userId, cancellationToken);
        }
    }
}
