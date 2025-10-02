using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Platform.Application.Utils;
using Platform.Domain.DTOs.Auth;
using Platform.Domain.Entities.Auth;
using Platform.Domain.Repositories;
using BC = BCrypt.Net.BCrypt;
using Platform.Domain.Repositories.Auth;

namespace Platform.Application.Core.Auth.Commands.Authentication
{
    public class LoginCommand : ILoginCommand
    {
        private readonly IRepositoryBase<User> _userRepository;
        private readonly IRepositoryBase<UserType> _userTypeRepository;
        private readonly IRepositoryBase<Session> _sessionRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IRepositoryBase<Permission> _permissionRepository;
        private readonly ILogger<LoginCommand> _logger;
        private readonly IConfiguration _configuration;

        public LoginCommand(IRepositoryBase<User> userRepository, IRepositoryBase<Session> sessionRepository, IConfiguration configuration, ILogger<LoginCommand> logger, IRepositoryBase<UserType> userTypeRepository, IUserRoleRepository userRoleRepository, IRolePermissionRepository rolePermissionRepository, IRepositoryBase<Permission> permissionRepository) 
        {
            _userRepository = userRepository;
            _userTypeRepository = userTypeRepository;
            _sessionRepository = sessionRepository;
            _userRoleRepository = userRoleRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _permissionRepository = permissionRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<LoginResponse?> Login(LoginRequest autorizacion, CancellationToken cancellationToken)
        {
            // Buscar usuario solo por email
            User? CurrentUser = await _userRepository.Find(x => x.Email == autorizacion.Email, cancellationToken);
            
            // Verificar si el usuario existe y la contraseña es correcta
            if (CurrentUser != null && !string.IsNullOrEmpty(CurrentUser.Password))
            {
                bool isPasswordValid = BC.Verify(autorizacion.Password, CurrentUser.Password);
                if (isPasswordValid)
                {
                    CurrentUser.UserType = await _userTypeRepository.Find(x => x.Id == CurrentUser.UserTypeId, cancellationToken);
                    _logger.LogInformation("Login: success");
                    string CurrentToken = await GetToken(CurrentUser, cancellationToken);
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

        /// <summary>
        /// Obtiene todos los permisos del usuario basado en sus roles
        /// </summary>
        private async Task<List<string>> GetUsSignosstermissionsAsync(Guid userId, CancellationToken cancellationToken)
        {
            var permissions = new HashSet<string>();

            // Obtener roles del usuario
            var userRoles = await _userRoleRepository.GetUserRolesAsync(userId, cancellationToken);

            // Obtener permisos de cada rol
            foreach (var userRole in userRoles)
            {
                var rolePermissions = await _rolePermissionRepository.GetPermissionsByRoleIdAsync(userRole.RoleId, cancellationToken);
                
                foreach (var rolePermission in rolePermissions)
                {
                    var permission = await _permissionRepository.Find(p => p.Id == rolePermission.PermissionId, cancellationToken);
                    if (permission != null && permission.Status)
                    {
                        permissions.Add(permission.Name);
                    }
                }
            }

            return permissions.ToList();
        }

        private async Task<string> GenerateTokenAsync(User user, CancellationToken cancellationToken)
        {
            string? key = _configuration.GetValue<string>("JwtSettings:key");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Obtener permisos del usuario
            List<string> usSignosstermissions = await GetUsSignosstermissionsAsync(user.Id, cancellationToken);

            // Crear los claims básicos
            var claims = new List<Claim>
            {
                new(CustomClaimTypes.UserId, user.Id.ToString()),
                new(CustomClaimTypes.UserName, user.Name),
                new(CustomClaimTypes.UserEmail, user.Email),
                new(CustomClaimTypes.UserTypeId, user.UserTypeId.ToString()),
                new(CustomClaimTypes.UserTypeName, user.UserType!.Name),
            };

            // Agregar permisos como claims
            foreach (var permission in usSignosstermissions)
            {
                claims.Add(new Claim(CustomClaimTypes.Permission, permission));
            }

            // Crear el token
            DateTime ExperiredDate = DateTime.Now.AddMinutes(60);
            JwtSecurityToken tokenJwt = new(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: ExperiredDate,
                signingCredentials: credentials
            );

            string Newtoken = new JwtSecurityTokenHandler().WriteToken(tokenJwt);

            //Se almacena el nuevo token como session
            if (!string.IsNullOrEmpty(Newtoken))
            {
                Session session = new()
                {
                    Id = Guid.NewGuid(),
                    SessionToken = Newtoken,
                    UserId = user.Id,
                    Expires = ExperiredDate
                };

                await _sessionRepository.Create(session, cancellationToken);
            }
            return Newtoken;
        }

        private async Task<string> RefreshSessionAsync(Session session, User user, CancellationToken cancellationToken)
        {
            // Eliminar sesión anterior
            await _sessionRepository.Delete((int)session.Id.GetHashCode(), cancellationToken);

            string currentToken = await GenerateTokenAsync(user, cancellationToken);
            return currentToken;
        }

        private async Task<string> GetToken(User user, CancellationToken cancellationToken)
        {
            var CurrentSession = await _sessionRepository.Find(x => x.UserId == user.Id && x.Expires > DateTime.Now, cancellationToken);
            if (CurrentSession != null)
            {
                if (CurrentSession.Expires.CompareTo(DateTime.Now) < 0)
                {
                    _logger.LogInformation("GetToken: Expiration Session UserId:" + user.Id);
                    return await RefreshSessionAsync(CurrentSession, user, cancellationToken);
                }
                return CurrentSession.SessionToken!;
            }
            else
            {
                string currentToken = await GenerateTokenAsync(user, cancellationToken);
                if (!string.IsNullOrEmpty(currentToken))
                {
                    return currentToken;
                }
            }
            return string.Empty;
        }
    }
}
