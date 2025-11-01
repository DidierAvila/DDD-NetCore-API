using Platform.Domain.DTOs.Auth;

namespace Platform.Application.Core.Auth.Commands.Authentication
{
    public interface IExternalLoginCommand
    {
        Task<LoginResponse?> ExternalLogin(ExternalLoginRequest request, CancellationToken cancellationToken);
    }
}