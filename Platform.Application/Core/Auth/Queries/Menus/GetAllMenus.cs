using AutoMapper;
using Microsoft.Extensions.Logging;
using Platform.Domain.DTOs.Auth;
using Platform.Domain.Repositories.Auth;

namespace Platform.Application.Core.Auth.Queries.Menus
{
    public class GetAllMenus
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllMenus> _logger;

        public GetAllMenus(IMenuRepository menuRepository, IMapper mapper, ILogger<GetAllMenus> logger)
        {
            _menuRepository = menuRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<MenuDto>> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var menus = await _menuRepository.GetMenusOrderedAsync(cancellationToken);
                return _mapper.Map<IEnumerable<MenuDto>>(menus);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all menus");
                throw;
            }
        }
    }
}
