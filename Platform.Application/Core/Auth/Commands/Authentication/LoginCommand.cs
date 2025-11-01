using Microsoft.Extensions.Logging;
using Platform.Application.Core.Auth.Commands.Tokens;
using Platform.Domain.DTOs.Auth;
using Platform.Domain.Entities.Auth;
using Platform.Domain.Repositories;
using BC = BCrypt.Net.BCrypt;

namespace Platform.Application.Core.Auth.Commands.Authentication
{
    public class LoginCommand : ILoginCommand
    {
        private readonly IRepositoryBase<User> _userRepository;
        private readonly IRepositoryBase<UserType> _userTypeRepository;
        private readonly ITokenCommand _tokenCommand;
        private readonly ILogger<LoginCommand> _logger;

        public LoginCommand(
            IRepositoryBase<User> userRepository, 
            ILogger<LoginCommand> logger, 
            IRepositoryBase<UserType> userTypeRepository,
            ITokenCommand tokenCommand) 
        {
            _userRepository = userRepository;
            _userTypeRepository = userTypeRepository;
            _logger = logger;
            _tokenCommand = tokenCommand;
        }

        public async Task<LoginResponse?> Login(LoginRequest autorizacion, CancellationToken cancellationToken)
        {
            // Buscar usuario solo por email
            User? CurrentUser = await _userRepository.Find(x => x.Email == autorizacion.Email, cancellationToken);

            if (!CurrentUser!.Status)
            {
                throw new UnauthorizedAccessException("Usuario inactivo. Contacte al administrador.");
            }

            // Verificar si el usuario existe y la contraseña es correcta
            if (CurrentUser != null && !string.IsNullOrEmpty(CurrentUser.Password))
            {
                bool isPasswordValid = BC.Verify(autorizacion.Password, CurrentUser.Password);
                if (isPasswordValid)
                {
                    CurrentUser.UserType = await _userTypeRepository.Find(x => x.Id == CurrentUser.UserTypeId, cancellationToken);
                    _logger.LogInformation("Login: success");
                    string CurrentToken = await _tokenCommand.GetToken(CurrentUser, cancellationToken);
                    LoginResponse loginResponse = new()
                    { Token = CurrentToken };

                    return loginResponse;
                }
                else
                {
                    _logger.LogWarning("Login: invalid password for user {Email}", autorizacion.Email);
                }
            }
            else
            {
                _logger.LogWarning("Login: user not found {Email}", autorizacion.Email);
            }
            return null;
        }
    }
}
