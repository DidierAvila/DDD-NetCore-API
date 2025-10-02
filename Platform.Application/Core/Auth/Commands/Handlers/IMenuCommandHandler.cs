using Platform.Domain.DTOs.Auth;

namespace Platform.Application.Core.Auth.Commands.Handlers
{
    public interface IMenuCommandHandler
    {
        Task<MenuDto> CreateMenuAsync(CreateMenuDto createMenuDto, CancellationToken cancellationToken = default);
        Task<MenuDto> UpdateMenuAsync(Guid id, UpdateMenuDto updateMenuDto, CancellationToken cancellationToken = default);
        Task<bool> DeleteMenuAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
