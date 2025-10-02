using Microsoft.Extensions.Logging;
using Platform.Domain.Repositories.Auth;

namespace Platform.Application.Services
{
    /// <summary>
    /// Servicio para manejar la invalidación de sesiones cuando se actualizan permisos de roles.
    /// </summary>
    public class SessionInvalidationService
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly ILogger<SessionInvalidationService> _logger;

        public SessionInvalidationService(
            ISessionRepository sessionRepository,
            ILogger<SessionInvalidationService> logger)
        {
            _sessionRepository = sessionRepository;
            _logger = logger;
        }

        /// <summary>
        /// Invalida todas las sesiones activas de usuarios que tienen un rol específico.
        /// </summary>
        /// <param name="roleId">ID del rol cuyos permisos fueron modificados</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Número de sesiones invalidadas</returns>
        public async Task<int> InvalidateSessionsByRoleAsync(Guid roleId, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Iniciando invalidación de sesiones para rol {RoleId}", roleId);

                var invalidatedCount = await _sessionRepository.DeleteSessionsByRoleIdAsync(roleId, cancellationToken);

                _logger.LogInformation(
                    "Invalidación completada. {InvalidatedCount} sesiones invalidadas para rol {RoleId}", 
                    invalidatedCount, roleId);

                return invalidatedCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al invalidar sesiones para rol {RoleId}", roleId);
                throw;
            }
        }

        /// <summary>
        /// Invalidar todas las sesiones de usuarios que tienen cualquiera de los roles especificados.
        /// </summary>
        /// <param name="roleIds">Lista de IDs de roles</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Número de sesiones invalidadas</returns>
        public async Task<int> InvalidateSessionsByMultipleRolesAsync(List<Guid> roleIds, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!roleIds.Any())
                {
                    _logger.LogWarning("Se intentó invalidar sesiones con una lista vacía de roles");
                    return 0;
                }

                _logger.LogInformation("Iniciando invalidación de sesiones para {RoleCount} roles", roleIds.Count);

                var totalInvalidated = await _sessionRepository.DeleteSessionsByRoleIdsAsync(roleIds, cancellationToken);

                _logger.LogInformation("Invalidación múltiple completada. {TotalInvalidated} sesiones invalidadas para {RoleCount} roles", 
                    totalInvalidated, roleIds.Count);

                return totalInvalidated;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al invalidar sesiones para múltiples roles");
                throw;
            }
        }

        /// <summary>
        /// Invalida una sesión específica de un usuario.
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Número de sesiones invalidadas para el usuario</returns>
        public async Task<int> InvalidateUserSessionsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Invalidando sesiones para usuario {UserId}", userId);

                var invalidatedCount = await _sessionRepository.DeleteSessionsByUserIdAsync(userId, cancellationToken);

                _logger.LogInformation(
                    "Invalidadas {InvalidatedCount} sesiones para usuario {UserId}", 
                    invalidatedCount, userId);

                return invalidatedCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al invalidar sesiones para usuario {UserId}", userId);
                throw;
            }
        }
    }
}
