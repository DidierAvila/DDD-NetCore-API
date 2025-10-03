using Platform.Domain.Entities.App;

namespace Platform.Domain.Repositories.App
{
    public interface ICountryRepository : IRepositoryBase<Country>
    {
        Task<IEnumerable<Country>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<Country>> GetCountriesByServiceIdAsync(Guid serviceId, CancellationToken cancellationToken = default);
    }
}