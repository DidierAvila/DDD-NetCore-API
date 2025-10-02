using Platform.Application.Core.Auth.Commands.Menus;
using Platform.Domain.DTOs.Auth;

namespace Platform.Application.Core.Auth.Commands.Handlers
{
    public class MenuCommandHandler : IMenuCommandHandler
    {
        private readonly CreateMenu _createMenu;
        private readonly UpdateMenu _updateMenu;
        private readonly DeleteMenu _deleteMenu;

        public MenuCommandHandler(
            CreateMenu createMenu,
            UpdateMenu updateMenu,
            DeleteMenu deleteMenu)
        {
            _createMenu = createMenu;
            _updateMenu = updateMenu;
            _deleteMenu = deleteMenu;
        }

        public async Task<MenuDto> CreateMenuAsync(CreateMenuDto createMenuDto, CancellationToken cancellationToken = default)
        {
            return await _createMenu.ExecuteAsync(createMenuDto, cancellationToken);
        }

        public async Task<MenuDto> UpdateMenuAsync(Guid id, UpdateMenuDto updateMenuDto, CancellationToken cancellationToken = default)
        {
            return await _updateMenu.ExecuteAsync(id, updateMenuDto, cancellationToken);
        }

        public async Task<bool> DeleteMenuAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _deleteMenu.ExecuteAsync(id, cancellationToken);
        }
    }
}
