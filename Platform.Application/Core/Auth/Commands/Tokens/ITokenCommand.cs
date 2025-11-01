using Platform.Domain.Entities.Auth;

namespace Platform.Application.Core.Auth.Commands.Tokens
{
    public interface ITokenCommand
    {
        Task<string> GetToken(User user, CancellationToken cancellationToken);
    }
}