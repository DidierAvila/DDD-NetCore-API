using Platform.Domain.Entities.App;

namespace Platform.Domain.Repositories.App
{
    public interface IServiceRepository : IRepositoryBase<Service>
    {
        Task<IEnumerable<Service>> GetActiveServicesAsync(CancellationToken cancellationToken = default);
        Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null);
        Task<Service?> GetByIdWithSupplierAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Service>> GetAllWithSupplierAsync(CancellationToken cancellationToken = default);
    }
}