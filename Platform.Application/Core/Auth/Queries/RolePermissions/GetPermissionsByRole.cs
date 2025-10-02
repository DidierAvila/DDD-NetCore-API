using AutoMapper;
using Platform.Domain.DTOs.Auth;
using Platform.Domain.Repositories.Auth;

namespace Platform.Application.Core.Auth.Queries.RolePermissions
{
    public class GetPermissionsByRole
    {
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IMapper _mapper;

        public GetPermissionsByRole(IRolePermissionRepository rolePermissionRepository, IMapper mapper)
        {
            _rolePermissionRepository = rolePermissionRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RolePermissionDto>> HandleAsync(Guid roleId, CancellationToken cancellationToken = default)
        {
            var entities = await _rolePermissionRepository.GetPermissionsByRoleIdAsync(roleId, cancellationToken);
            return _mapper.Map<IEnumerable<RolePermissionDto>>(entities);
        }
    }
}
