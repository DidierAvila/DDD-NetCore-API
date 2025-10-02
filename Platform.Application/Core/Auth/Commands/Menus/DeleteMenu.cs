using Microsoft.Extensions.Logging;
using Platform.Domain.Repositories.Auth;

namespace Platform.Application.Core.Auth.Commands.Menus
{
    public class DeleteMenu
    {
        private readonly IMenuRepository _menuRepository;
        private readonly ILogger<DeleteMenu> _logger;

        public DeleteMenu(IMenuRepository menuRepository, ILogger<DeleteMenu> logger)
        {
            _menuRepository = menuRepository;
            _logger = logger;
        }

        public async Task<bool> ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var menu = await _menuRepository.GetByID(id, cancellationToken);
                if (menu == null)
                {
                    return false;
                }

                // Verificar si tiene menús hijos
                var hasChildren = await _menuRepository.HasChildMenusAsync(id, cancellationToken);
                if (hasChildren)
                {
                    throw new ArgumentException("Cannot delete menu that has child menus");
                }

                await _menuRepository.Delete(id, cancellationToken);

                _logger.LogInformation("Menu deleted successfully with ID: {MenuId}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting menu with ID: {MenuId}", id);
                throw;
            }
        }
    }
}
