using AutoMapper;
using Microsoft.Extensions.Logging;
using Platform.Domain.DTOs.Auth;
using Platform.Domain.Entities.Auth;
using Platform.Domain.Repositories.Auth;

namespace Platform.Application.Core.Auth.Commands.Menus
{
    public class CreateMenu
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateMenu> _logger;

        public CreateMenu(IMenuRepository menuRepository, IMapper mapper, ILogger<CreateMenu> logger)
        {
            _menuRepository = menuRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<MenuDto> ExecuteAsync(CreateMenuDto createMenuDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var menu = _mapper.Map<Menu>(createMenuDto);
            menu.Id = Guid.NewGuid();
            menu.CreatedAt = DateTime.Now;

            var createdMenu = await _menuRepository.Create(menu, cancellationToken);

                _logger.LogInformation("Menu created successfully with ID: {MenuId}", createdMenu.Id);
                return _mapper.Map<MenuDto>(createdMenu);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating menu with label: {Label}", createMenuDto.Label);
                throw;
            }
        }
    }
}
