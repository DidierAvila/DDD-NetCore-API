using Platform.Domain.Entities.App;

namespace Platform.Domain.Repositories.App
{
    public interface IServiceRepository : IRepositoryBase<Service>
    {
        Task<IEnumerable<Service>> GetActiveServicesAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<Service>> GetServicesByMethodAsync(string method, CancellationToken cancellationToken = default);
        Task<Service?> GetByEndpointAsync(string endpoint, CancellationToken cancellationToken = default);
    }
}