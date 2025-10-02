using AutoMapper;
using Microsoft.Extensions.Logging;
using Platform.Domain.DTOs.Auth;
using Platform.Domain.Repositories.Auth;

namespace Platform.Application.Core.Auth.Queries.Menus
{
    public class GetMenuById
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetMenuById> _logger;

        public GetMenuById(IMenuRepository menuRepository, IMapper mapper, ILogger<GetMenuById> logger)
        {
            _menuRepository = menuRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<MenuDto?> ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var menu = await _menuRepository.GetByID(id, cancellationToken);
                if (menu == null)
                {
                    _logger.LogWarning("Menu with ID {MenuId} not found.", id);
                    return null;
                }

                return _mapper.Map<MenuDto>(menu);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving menu with ID: {MenuId}", id);
                throw;
            }
        }
    }
}
