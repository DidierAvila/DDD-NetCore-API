using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Platform.Domain.Entities.Auth;
using Platform.Domain.Repositories.Auth;
using Platform.Infrastructure.DbContexts;

namespace Platform.Infrastructure.Repositories.Auth
{
    public class MenuPermissionRepository : RepositoryBase<MenuPermission>, IMenuPermissionRepository
    {
        public MenuPermissionRepository(PlatformDbContext context, ILogger<MenuPermissionRepository> logger) : base(context, logger)
        {
        }

        public async Task<IEnumerable<MenuPermission>> GetPermissionsByMenuIdAsync(Guid menuId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<MenuPermission>()
                .Where(mp => mp.MenuId == menuId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<MenuPermission>> GetMenusByPermissionIdAsync(Guid permissionId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<MenuPermission>()
                .Where(mp => mp.PermissionId == permissionId)
                .ToListAsync(cancellationToken);
        }

        public async Task<MenuPermission?> GetByMenuAndPermissionAsync(Guid menuId, Guid permissionId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<MenuPermission>()
                .FirstOrDefaultAsync(mp => mp.MenuId == menuId && mp.PermissionId == permissionId, cancellationToken);
        }

        public async Task<bool> ExistsAsync(Guid menuId, Guid permissionId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<MenuPermission>()
                .AnyAsync(mp => mp.MenuId == menuId && mp.PermissionId == permissionId, cancellationToken);
        }

        public async Task<IEnumerable<MenuPermission>> GetMenuPermissionsWithDetailsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<MenuPermission>()
                .ToListAsync(cancellationToken);
        }

        public async Task RemovePermissionFromMenuAsync(Guid menuId, Guid permissionId, CancellationToken cancellationToken = default)
        {
            var menuPermission = await GetByMenuAndPermissionAsync(menuId, permissionId, cancellationToken);
            if (menuPermission != null)
            {
                _context.Set<MenuPermission>().Remove(menuPermission);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task RemoveAllPermissionsFromMenuAsync(Guid menuId, CancellationToken cancellationToken = default)
        {
            var menuPermissions = await _context.Set<MenuPermission>()
                .Where(mp => mp.MenuId == menuId)
                .ToListAsync(cancellationToken);

            if (menuPermissions.Any())
            {
                _context.Set<MenuPermission>().RemoveRange(menuPermissions);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<MenuPermission?> GetByCompositeIdAsync(Guid menuId, Guid permissionId, CancellationToken cancellationToken = default)
        {
            return await GetByMenuAndPermissionAsync(menuId, permissionId, cancellationToken);
        }
    }
}
