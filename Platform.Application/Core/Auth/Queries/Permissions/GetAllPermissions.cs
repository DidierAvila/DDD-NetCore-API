using AutoMapper;
using Platform.Domain.Repositories;
using Platform.Domain.DTOs.Auth;
using Platform.Domain.Entities.Auth;

namespace Platform.Application.Core.Auth.Queries.Permissions
{
    public class GetAllPermissions
    {
        private readonly IRepositoryBase<Platform.Domain.Entities.Auth.Permission> _permissionRepository;
        private readonly IMapper _mapper;

        public GetAllPermissions(IRepositoryBase<Platform.Domain.Entities.Auth.Permission> permissionRepository, IMapper mapper)
        {
            _permissionRepository = permissionRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PermissionDto>> HandleAsync(CancellationToken cancellationToken)
        {
            var permissions = await _permissionRepository.GetAll(cancellationToken);
            return _mapper.Map<IEnumerable<PermissionDto>>(permissions);
        }
    }
}
