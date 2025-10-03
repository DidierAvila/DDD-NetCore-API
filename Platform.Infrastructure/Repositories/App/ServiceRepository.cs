using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Platform.Domain.Entities.App;
using Platform.Domain.Entities.Auth;
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

        public async Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null)
        {
            var query = _context.Services.Where(s => s.Name == name);
            
            if (excludeId.HasValue)
            {
                query = query.Where(s => s.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<Service?> GetByIdWithSupplierAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Services
                .Where(s => s.Id == id)
                .Select(s => new Service
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    Status = s.Status,
                    HourlyValue = s.HourlyValue,
                    SupplierId = s.SupplierId,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt,
                    // Incluimos una propiedad de navegación temporal para el proveedor
                    // que será mapeada a SupplierName en el DTO
                    Supplier = _context.Users.FirstOrDefault(u => u.Id == s.SupplierId)
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<Service>> GetAllWithSupplierAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Services
                .Select(s => new Service
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    Status = s.Status,
                    HourlyValue = s.HourlyValue,
                    SupplierId = s.SupplierId,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt,
                    // Incluimos una propiedad de navegación temporal para el proveedor
                    // que será mapeada a SupplierName en el DTO
                    Supplier = _context.Users.FirstOrDefault(u => u.Id == s.SupplierId)
                })
                .ToListAsync(cancellationToken);
        }
    }
}