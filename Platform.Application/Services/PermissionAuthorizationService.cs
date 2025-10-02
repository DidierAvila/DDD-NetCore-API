using Platform.Domain.Entities.Auth;
using Platform.Domain.Repositories;
using Platform.Domain.Repositories.Auth;

namespace Platform.Application.Services
{
    /// <summary>
    /// Servicio para manejar la autorización de permisos de usuarios.
    /// </summary>
    public class PermissionAuthorizationService
    {
        private readonly IRepositoryBase<User> _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IRepositoryBase<Permission> _permissionRepository;
        private readonly IRepositoryBase<Role> _roleRepository;

        public PermissionAuthorizationService(
            IRepositoryBase<User> userRepository,
            IUserRoleRepository userRoleRepository,
            IRolePermissionRepository rolePermissionRepository,
            IRepositoryBase<Permission> permissionRepository,
            IRepositoryBase<Role> roleRepository)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _permissionRepository = permissionRepository;
            _roleRepository = roleRepository;
        }

        /// <summary>
        /// Verifica si el usuario tiene el permiso requerido.
        /// </summary>
        /// <param name="userId">ID del usuario.</param>
        /// <param name="permission">Permiso a verificar.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>True si el usuario tiene el permiso, false en caso contrario.</returns>
        public async Task<bool> CheckUserPermissionAsync(Guid userId, string permission, CancellationToken cancellationToken = default)
        {
            try
            {
                // Verificar que el usuario existe
                var user = await _userRepository.Find(u => u.Id == userId, cancellationToken);
                if (user == null)
                    return false;

                // Obtener roles del usuario
                var userRoles = await _userRoleRepository.GetUserRolesAsync(userId, cancellationToken);
                
                // Verificar permisos en cada rol activo
                foreach (var userRole in userRoles)
                {
                    // Verificar que el rol esté activo
                    var role = await _roleRepository.Find(r => r.Id == userRole.RoleId, cancellationToken);
                    if (role == null || !role.Status)
                        continue;

                    // Obtener permisos del rol
                    var rolePermissions = await _rolePermissionRepository.GetPermissionsByRoleIdAsync(userRole.RoleId, cancellationToken);
                    
                    foreach (var rolePermission in rolePermissions)
                    {
                        var permissionEntity = await _permissionRepository.Find(p => p.Id == rolePermission.PermissionId, cancellationToken);
                        if (permissionEntity != null && permissionEntity.Status && permissionEntity.Name == permission)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
