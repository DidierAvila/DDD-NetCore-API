using Platform.Domain.DTOs.Auth;

namespace Platform.Application.Core.Auth.Commands.Authentication
{
    public interface ILoginCommand
    {
        Task<LoginResponse?> Login(LoginRequest autorizacion, CancellationToken cancellationToken);
    }
}
