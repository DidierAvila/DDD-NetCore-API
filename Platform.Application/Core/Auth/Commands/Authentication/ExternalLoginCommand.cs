using AutoMapper;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Platform.Application.Core.Auth.Commands.Handlers;
using Platform.Application.Core.Auth.Commands.Tokens;
using Platform.Domain.DTOs.Auth;
using Platform.Domain.Entities.Auth;
using Platform.Domain.Repositories;
using Platform.Domain.Repositories.Auth;
using Platform.Domain.Utils;

namespace Platform.Application.Core.Auth.Commands.Authentication
{
    public class ExternalLoginCommand : IExternalLoginCommand
    {
        private readonly IRepositoryBase<User> _userRepository;
        private readonly IRepositoryBase<UserType> _userTypeRepository;
        private readonly ILogger<ExternalLoginCommand> _logger;
        private readonly IConfiguration _configuration;
        private readonly IUserCommandHandler _userCommandHandler;
        private readonly ITokenCommand _tokenCommand;
        private readonly IRepositoryBase<Role> _roleRepository;
        private readonly IMapper _mapper;

        public ExternalLoginCommand(
            IRepositoryBase<User> userRepository,
            IConfiguration configuration,
            ILogger<ExternalLoginCommand> logger,
            IRepositoryBase<UserType> userTypeRepository,
            IUserRoleRepository userRoleRepository,
            IUserCommandHandler userCommandHandler,
            IRepositoryBase<Role> roleRepository,
            IMapper mapper,
            ITokenCommand tokenCommand)
        {
            _userRepository = userRepository;
            _userTypeRepository = userTypeRepository;
            _configuration = configuration;
            _logger = logger;
            _userCommandHandler = userCommandHandler;
            _tokenCommand = tokenCommand;
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<LoginResponse?> ExternalLogin(ExternalLoginRequest request, CancellationToken cancellationToken)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.IdToken))
            {
                throw new ArgumentException("El id_token es requerido para el login externo.");
            }

            var provider = (request.Provider ?? "google").ToLowerInvariant();
            if (provider != "google")
            {
                throw new NotSupportedException("Proveedor externo no soportado. Solo 'google'.");
            }

            var clientId = _configuration.GetValue<string>("GoogleOAuth:ClientId");
            if (string.IsNullOrWhiteSpace(clientId))
            {
                throw new InvalidOperationException("GoogleOAuth:ClientId no está configurado.");
            }

            GoogleJsonWebSignature.Payload payload;
            try
            {
                var validationSettings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { clientId }
                };
                payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, validationSettings);
            }
            catch (InvalidJwtException ex)
            {
                _logger.LogWarning(ex, "Token de Google inválido");
                throw new UnauthorizedAccessException("Token de Google inválido.", ex);
            }

            if (payload == null)
            {
                throw new UnauthorizedAccessException("No se pudo validar el token de Google.");
            }

            if (!payload.EmailVerified)
            {
                throw new UnauthorizedAccessException("El correo de Google no está verificado.");
            }

            var email = payload.Email;
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new InvalidOperationException("El token de Google no contiene email.");
            }

            var user = await _userRepository.Find(x => x.Email == email, cancellationToken);
            if (user == null)
            {
                // Obtiene el tipo de usuario
                if (!request.UserType.HasValue)
                {
                    throw new InvalidOperationException("Se debe definir el tipo de usuario a crear.");
                }
                string currentUserType = UtilsEnum.GetDisplayName(request.UserType.Value);

                // Crear usuario automáticamente usando el tipo solicitado o por defecto
                var selectedUserType = await _userTypeRepository.Find(x => x.Status && x.Name == currentUserType, cancellationToken);
                var selectedRole = await _roleRepository.Find(role => role.Name == currentUserType, cancellationToken);
                if (selectedUserType == null)
                {
                    throw new InvalidOperationException("No hay UserType activo para asignar al nuevo usuario. Cree al menos uno.");
                }

                var displayName = !string.IsNullOrWhiteSpace(payload.Name)
                    ? payload.Name
                    : $"{payload.GivenName} {payload.FamilyName}".Trim();
                if (string.IsNullOrWhiteSpace(displayName))
                {
                    displayName = email.Split('@')[0];
                }

                var createDto = new CreateUserDto
                {
                    Name = displayName,
                    Email = email,
                    UserTypeId = selectedUserType.Id,
                    Image = payload.Picture,
                    RoleIds = selectedRole != null ? [selectedRole!.Id ] : [],
                    Status = true
                };

                var userDto = await _userCommandHandler.CreateUser(createDto, cancellationToken);
                if (userDto == null && userDto!.Id == Guid.Empty)
                {
                    throw new InvalidOperationException("Error creando el usuario automáticamente.");
                }
                user = _mapper.Map<User>(userDto);
                user.UserType = selectedUserType;
            }

            if (!user.Status)
            {
                throw new UnauthorizedAccessException("Usuario inactivo. Contacte al administrador.");
            }

            if (user.UserType != null)
            {
                user.UserType = await _userTypeRepository.Find(x => x.Id == user.UserTypeId, cancellationToken);
            }

            var token = await _tokenCommand.GetToken(user, cancellationToken);
            return new LoginResponse { Token = token };
        }
    }
}