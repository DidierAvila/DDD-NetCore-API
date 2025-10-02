using Platform.Domain.Entities.Auth;

namespace Platform.Domain.Repositories.Auth
{
    public interface IMenuPermissionRepository : IRepositoryBase<MenuPermission>
    {
        Task<IEnumerable<MenuPermission>> GetPermissionsByMenuIdAsync(Guid menuId, CancellationToken cancellationToken = default);
        Task<IEnumerable<MenuPermission>> GetMenusByPermissionIdAsync(Guid permissionId, CancellationToken cancellationToken = default);
        Task<MenuPermission?> GetByMenuAndPermissionAsync(Guid menuId, Guid permissionId, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Guid menuId, Guid permissionId, CancellationToken cancellationToken = default);
        Task<IEnumerable<MenuPermission>> GetMenuPermissionsWithDetailsAsync(CancellationToken cancellationToken = default);
        Task RemovePermissionFromMenuAsync(Guid menuId, Guid permissionId, CancellationToken cancellationToken = default);
        Task RemoveAllPermissionsFromMenuAsync(Guid menuId, CancellationToken cancellationToken = default);
        Task<MenuPermission?> GetByCompositeIdAsync(Guid menuId, Guid permissionId, CancellationToken cancellationToken = default);
    }
}
