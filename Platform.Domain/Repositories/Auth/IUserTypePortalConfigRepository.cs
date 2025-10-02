using Platform.Domain.Entities.Auth;

namespace Platform.Domain.Repositories.Auth
{
    public interface IUserTypePortalConfigRepository
    {
        Task<IEnumerable<UserTypePortalConfig>> GetByUserTypeIdAsync(Guid userTypeId, CancellationToken cancellationToken = default);
        Task<UserTypePortalConfig?> GetByUserTypeIdAsync(Guid userTypeId, Guid configId, CancellationToken cancellationToken = default);
        Task<UserTypePortalConfig> UpsertConfigAsync(UserTypePortalConfig config, CancellationToken cancellationToken = default);
        Task<bool> DeleteByUserTypeAsync(Guid userTypeId, CancellationToken cancellationToken = default);
    }
}
