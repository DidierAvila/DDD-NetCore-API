using AutoMapper;
using Platform.Domain.DTOs.Auth;
using Platform.Domain.Entities.Auth;
using Platform.Domain.Repositories;

namespace Platform.Application.Core.Auth.Queries.Permissions
{
    public class GetActivePermissions
    {
        private readonly IRepositoryBase<Platform.Domain.Entities.Auth.Permission> _permissionRepository;
        private readonly IMapper _mapper;

        public GetActivePermissions(IRepositoryBase<Platform.Domain.Entities.Auth.Permission> permissionRepository, IMapper mapper)
        {
            _permissionRepository = permissionRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PermissionDto>> HandleAsync(CancellationToken cancellationToken)
        {
            var activePermissions = await _permissionRepository.Finds(x => x.Status == true, cancellationToken);
            return _mapper.Map<IEnumerable<PermissionDto>>(activePermissions);
        }
    }
}
