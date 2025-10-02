using Platform.Application.Core.Auth.Queries.Menus;
using Platform.Domain.DTOs.Auth;

namespace Platform.Application.Core.Auth.Queries.Handlers
{
    public class MenuQueryHandler : IMenuQueryHandler
    {
        private readonly GetMenuById _getMenuById;
        private readonly GetAllMenus _getAllMenus;
        private readonly GetMenuTree _getMenuTree;

        public MenuQueryHandler(
            GetMenuById getMenuById,
            GetAllMenus getAllMenus,
            GetMenuTree getMenuTree)
        {
            _getMenuById = getMenuById;
            _getAllMenus = getAllMenus;
            _getMenuTree = getMenuTree;
        }

        public async Task<MenuDto?> GetMenuByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _getMenuById.ExecuteAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<MenuDto>> GetAllMenusAsync(CancellationToken cancellationToken = default)
        {
            return await _getAllMenus.ExecuteAsync(cancellationToken);
        }

        public async Task<IEnumerable<MenuTreeDto>> GetMenuTreeAsync(bool activeOnly = false, CancellationToken cancellationToken = default)
        {
            return await _getMenuTree.ExecuteAsync(activeOnly, cancellationToken);
        }
    }
}
