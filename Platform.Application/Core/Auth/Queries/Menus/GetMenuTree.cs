using AutoMapper;
using Microsoft.Extensions.Logging;
using Platform.Domain.DTOs.Auth;
using Platform.Domain.Repositories.Auth;

namespace Platform.Application.Core.Auth.Queries.Menus
{
    public class GetMenuTree
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetMenuTree> _logger;

        public GetMenuTree(IMenuRepository menuRepository, IMapper mapper, ILogger<GetMenuTree> logger)
        {
            _menuRepository = menuRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<MenuTreeDto>> ExecuteAsync(bool activeOnly = false, CancellationToken cancellationToken = default)
        {
            try
            {
                var allMenus = activeOnly 
                    ? await _menuRepository.GetActiveMenusAsync(cancellationToken)
                    : await _menuRepository.GetMenusOrderedAsync(cancellationToken);

                var menuList = allMenus.ToList();
                var menuTree = BuildMenuTree(menuList, Guid.Empty);

                return _mapper.Map<IEnumerable<MenuTreeDto>>(menuTree);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error building menu tree");
                throw;
            }
        }

        private IEnumerable<Domain.Entities.Auth.Menu> BuildMenuTree(List<Domain.Entities.Auth.Menu> allMenus, Guid parentId)
        {
            return allMenus
                .Where(m => m.ParentId == parentId)
                .OrderBy(m => m.Order)
                .Select(menu => new Domain.Entities.Auth.Menu
                {
                    Id = menu.Id,
                    Label = menu.Label,
                    Icon = menu.Icon,
                    Route = menu.Route,
                    Order = menu.Order,
                    IsGroup = menu.IsGroup,
                    ParentId = menu.ParentId,
                    Status = menu.Status
                })
                .ToList();
        }
    }
}
