using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Platform.Domain.Entities.App;
using Platform.Domain.Repositories.App;
using Platform.Infrastructure.DbContexts;

namespace Platform.Infrastructure.Repositories.App
{
    public class ServiceRepository : RepositoryBase<Service>, IServiceRepository
    {
        public ServiceRepository(PlatformDbContext context, ILogger<RepositoryBase<Service>> logger) : base(context, logger) { }

        public async Task<IEnumerable<Service>> GetActiveServicesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Services
                .Where(s => s.Status)
                .OrderBy(s => s.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Service>> GetServicesByMethodAsync(string method, CancellationToken cancellationToken = default)
        {
            return await _context.Services
                .Where(s => s.Method == method)
                .OrderBy(s => s.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<Service?> GetByEndpointAsync(string endpoint, CancellationToken cancellationToken = default)
        {
            return await _context.Services
                .FirstOrDefaultAsync(s => s.Endpoint == endpoint, cancellationToken);
        }

        public async Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null)
        {
            var query = _context.Services.Where(s => s.Name == name);
            
            if (excludeId.HasValue)
            {
                query = query.Where(s => s.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<bool> ExistsByEndpointAndMethodAsync(string endpoint, string method, Guid? excludeId = null)
        {
            var query = _context.Services.Where(s => s.Endpoint == endpoint && s.Method == method);
            
            if (excludeId.HasValue)
            {
                query = query.Where(s => s.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }
    }
}