using Platform.Domain.DTOs.Auth;

namespace Platform.Application.Core.Auth.Queries.Handlers
{
    public interface IMenuQueryHandler
    {
        Task<MenuDto?> GetMenuByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<MenuDto>> GetAllMenusAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<MenuTreeDto>> GetMenuTreeAsync(bool activeOnly = false, CancellationToken cancellationToken = default);
    }
}
