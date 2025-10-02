using AutoMapper;
using Platform.Domain.Repositories;
using Platform.Domain.DTOs.Auth;
using Platform.Domain.Entities.Auth;
using Platform.Domain.Repositories.Auth;

namespace Platform.Application.Core.Auth.Queries.Users
{
    public class GetUserById
    {
        private readonly IRepositoryBase<User> _userRepository;
        private readonly IRepositoryBase<UserType> _userTypeRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IMapper _mapper;

        public GetUserById(IRepositoryBase<User> userRepository, IRepositoryBase<UserType> userTypeRepository, IUserRoleRepository userRoleRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _userTypeRepository = userTypeRepository;
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
        }

        public async Task<UserDto?> HandleAsync(Guid id, CancellationToken cancellationToken)
        {
            var user = await _userRepository.Find(x => x.Id == id, cancellationToken);
            if (user == null)
                return null;

            // Obtener el UserType por separado
            var userType = await _userTypeRepository.Find(x => x.Id == user.UserTypeId, cancellationToken);
            
            // Map Entity to DTO using AutoMapper
            var userDto = _mapper.Map<UserDto>(user);
            
            // Asignar el nombre del tipo de usuario
            userDto.UserTypeName = userType?.Name;

            // Cargar roles del usuario
            userDto.Roles = await LoadUserRoles(user.Id, cancellationToken);
            
            return userDto;
        }

        private async Task<List<RoleDropdownDto>> LoadUserRoles(Guid userId, CancellationToken cancellationToken)
        {
            var userRoles = await _userRoleRepository.GetUserRolesWithDetailsAsync(userId, cancellationToken);
            return _mapper.Map<List<RoleDropdownDto>>(userRoles);
        }
    }
}
