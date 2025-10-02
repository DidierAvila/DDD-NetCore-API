using Platform.Domain.DTOs.Auth;

namespace Platform.Application.Core.Auth.Queries.Handlers
{
    public interface IUserMeQueryHandler
    {
        Task<UserMeResponseDto> GetUserMe(Guid userId, CancellationToken cancellationToken = default);
    }
}
