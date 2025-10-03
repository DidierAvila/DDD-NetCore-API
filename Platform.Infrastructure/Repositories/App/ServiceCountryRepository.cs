using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Platform.Domain.Entities.App;
using Platform.Domain.Repositories.App;
using Platform.Infrastructure.DbContexts;

namespace Platform.Infrastructure.Repositories.App
{
    public class ServiceCountryRepository : RepositoryBase<ServiceCountry>, IServiceCountryRepository
    {
        public ServiceCountryRepository(PlatformDbContext context, ILogger<RepositoryBase<ServiceCountry>> logger) 
            : base(context, logger) { }

        public async Task<bool> AssignCountriesToServiceAsync(Guid serviceId, List<string> countryCodes)
        {
            try
            {
                // Verificar que el servicio existe
                var serviceExists = await _context.Services.AnyAsync(s => s.Id == serviceId);
                if (!serviceExists)
                {
                    return false;
                }

                // Obtener relaciones existentes
                var existingRelations = await _context.ServiceCountries
                    .Where(sc => sc.ServiceId == serviceId)
                    .ToListAsync();
                
                var existingCountryCodes = existingRelations.Select(r => r.CountryCode).ToHashSet();
                
                // Agregar nuevas relaciones
                foreach (var countryCode in countryCodes)
                {
                    if (!existingCountryCodes.Contains(countryCode))
                    {
                        _context.ServiceCountries.Add(new ServiceCountry
                        {
                            ServiceId = serviceId,
                            CountryCode = countryCode
                        });
                    }
                }
                
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al asignar países al servicio {serviceId}");
                return false;
            }
        }

        public async Task<bool> RemoveCountriesFromServiceAsync(Guid serviceId, List<string> countryCodes)
        {
            try
            {
                var relationsToRemove = await _context.ServiceCountries
                    .Where(sc => sc.ServiceId == serviceId && countryCodes.Contains(sc.CountryCode))
                    .ToListAsync();
                
                if (relationsToRemove.Any())
                {
                    _context.ServiceCountries.RemoveRange(relationsToRemove);
                    await _context.SaveChangesAsync();
                }
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar países del servicio {serviceId}");
                return false;
            }
        }
    }
}