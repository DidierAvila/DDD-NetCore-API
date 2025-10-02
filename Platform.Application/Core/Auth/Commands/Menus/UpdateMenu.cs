using AutoMapper;
using Microsoft.Extensions.Logging;
using Platform.Domain.DTOs.Auth;
using Platform.Domain.Repositories.Auth;

namespace Platform.Application.Core.Auth.Commands.Menus
{
    public class UpdateMenu
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateMenu> _logger;

        public UpdateMenu(IMenuRepository menuRepository, IMapper mapper, ILogger<UpdateMenu> logger)
        {
            _menuRepository = menuRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<MenuDto> ExecuteAsync(Guid id, UpdateMenuDto updateMenuDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var existingMenu = await _menuRepository.GetByID(id, cancellationToken);
                if (existingMenu == null)
                {
                    throw new KeyNotFoundException($"Menu with ID {id} not found");
                }

                _mapper.Map(updateMenuDto, existingMenu);
                existingMenu.UpdatedAt = DateTime.Now;

                await _menuRepository.Update(existingMenu, cancellationToken);

                _logger.LogInformation("Menu updated successfully with ID: {MenuId}", existingMenu.Id);
                return _mapper.Map<MenuDto>(existingMenu);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating menu with ID: {MenuId}", id);
                throw;
            }
        }
    }
}
