using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Platform.Domain.Entities.App;
using Platform.Domain.Repositories.App;
using Platform.Infrastructure.DbContexts;

namespace Platform.Infrastructure.Repositories.App
{
    public class CountryRepository : RepositoryBase<Country>, ICountryRepository
    {
        public CountryRepository(PlatformDbContext context, ILogger<RepositoryBase<Country>> logger) 
            : base(context, logger) { }

        public async Task<IEnumerable<Country>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Countries
                .OrderBy(c => c.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Country>> GetCountriesByServiceIdAsync(Guid serviceId, CancellationToken cancellationToken = default)
        {
            return await _context.ServiceCountries
                .Where(sc => sc.ServiceId == serviceId)
                .Include(sc => sc.Country)
                .Select(sc => sc.Country)
                .ToListAsync(cancellationToken);
        }
    }
}