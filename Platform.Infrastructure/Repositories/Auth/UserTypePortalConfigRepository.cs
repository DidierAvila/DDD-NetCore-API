using Microsoft.EntityFrameworkCore;
using Platform.Domain.Entities.Auth;
using Platform.Domain.Repositories.Auth;
using Platform.Infrastructure.DbContexts;

namespace Platform.Infrastructure.Repositories.Auth
{
    public class UserTypePortalConfigRepository : IUserTypePortalConfigRepository
    {
        private readonly PlatformDbContext _context;

        public UserTypePortalConfigRepository(PlatformDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserTypePortalConfig>> GetByUserTypeIdAsync(Guid userTypeId, CancellationToken cancellationToken = default)
        {
            return await _context.UserTypePortalConfigs
                .Where(c => c.UserTypeId == userTypeId)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<UserTypePortalConfig?> GetByUserTypeIdAsync(Guid userTypeId, Guid configId, CancellationToken cancellationToken = default)
        {
            return await _context.UserTypePortalConfigs
                .FirstOrDefaultAsync(c => c.UserTypeId == userTypeId && c.Id == configId, cancellationToken);
        }

        public async Task<UserTypePortalConfig> UpsertConfigAsync(UserTypePortalConfig config, CancellationToken cancellationToken = default)
        {
            var existing = await _context.UserTypePortalConfigs
                .FirstOrDefaultAsync(c => c.UserTypeId == config.UserTypeId, cancellationToken);
            
            if (existing != null)
            {
                // Actualizar configuración existente
                existing.Status = config.Status;
                existing.Theme = config.Theme;
                existing.AdditionalConfig = config.AdditionalConfig;
                existing.DefaultLandingPage = config.DefaultLandingPage;
                existing.LogoUrl = config.LogoUrl;
                existing.Language = config.Language;
                existing.UpdatedAt = DateTime.Now;
                
                _context.UserTypePortalConfigs.Update(existing);
                await _context.SaveChangesAsync(cancellationToken);
                return existing;
            }
            else
            {
                // Crear nueva configuración
                config.Id = Guid.NewGuid();
                config.CreatedAt = DateTime.Now;
                
                _context.UserTypePortalConfigs.Add(config);
                await _context.SaveChangesAsync(cancellationToken);
                return config;
            }
        }

        public async Task<bool> DeleteByUserTypeAsync(Guid userTypeId, CancellationToken cancellationToken = default)
        {
            var config = await _context.UserTypePortalConfigs
                .FirstOrDefaultAsync(c => c.UserTypeId == userTypeId, cancellationToken);
            
            if (config != null)
            {
                _context.UserTypePortalConfigs.Remove(config);
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
            
            return false;
        }
    }
}
