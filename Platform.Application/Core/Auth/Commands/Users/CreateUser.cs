using AutoMapper;
using Platform.Domain.Repositories;
using Platform.Domain.DTOs.Auth;
using Platform.Domain.Entities.Auth;
using BC = BCrypt.Net.BCrypt;
using Platform.Domain.Repositories.Auth;

namespace Platform.Application.Core.Auth.Commands.Users
{
    public class CreateUser
    {
        private readonly IRepositoryBase<User> _userRepository;
        private readonly IRepositoryBase<UserType> _userTypeRepository;
        private readonly IRepositoryBase<Role> _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IMapper _mapper;

        public CreateUser(IRepositoryBase<User> userRepository, IRepositoryBase<UserType> userTypeRepository, IRepositoryBase<Role> roleRepository, IUserRoleRepository userRoleRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _userTypeRepository = userTypeRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> HandleAsync(CreateUserDto createUserDto, CancellationToken cancellationToken)
        {
            // Validations
            if (string.IsNullOrWhiteSpace(createUserDto.Email))
                throw new ArgumentException("Email is required");

            if (createUserDto.UserTypeId == Guid.Empty)
                throw new ArgumentException("UserTypeId is required");

            // Check if user already exists
            var existingUser = await _userRepository.Find(x => x.Email == createUserDto.Email, cancellationToken);
            if (existingUser != null)
                throw new InvalidOperationException("User with this email already exists");

            // Map DTO to Entity using AutoMapper
            var user = _mapper.Map<User>(createUserDto);

            // Encrypt password before saving
            if (!string.IsNullOrEmpty(user.Password))
            {
                user.Password = BC.HashPassword(user.Password, 12);
            }

            // Create user in repository
            var createdUser = await _userRepository.Create(user, cancellationToken);

            // Asignar roles si se proporcionaron
            if (createUserDto.RoleIds != null && createUserDto.RoleIds.Any())
            {
                await AssignRolesToUser(createdUser.Id, createUserDto.RoleIds, cancellationToken);
            }

            // Obtener el UserType para incluir el nombre
            var userType = await _userTypeRepository.Find(x => x.Id == createdUser.UserTypeId, cancellationToken);

            // Map Entity to DTO using AutoMapper
            var userDto = _mapper.Map<UserDto>(createdUser);
            userDto.UserTypeName = userType?.Name;

            // Cargar roles asignados para incluir en la respuesta
            if (createUserDto.RoleIds != null && createUserDto.RoleIds.Any())
            {
                userDto.Roles = await LoadUserRoles(createdUser.Id, cancellationToken);
            }

            return userDto;
        }

        /// <summary>
        /// Asigna m�ltiples roles a un usuario reci�n creado
        /// </summary>
        private async Task AssignRolesToUser(Guid userId, List<Guid> roleIds, CancellationToken cancellationToken)
        {
            // Validar que todos los roles existen
            var validRoles = new List<Role>();
            foreach (var roleId in roleIds)
            {
                var role = await _roleRepository.Find(r => r.Id == roleId && r.Status, cancellationToken);
                if (role != null)
                {
                    validRoles.Add(role);
                }
            }

            // Asignar los roles v�lidos al usuario
            if (validRoles.Any())
            {
                foreach (var role in validRoles)
                {
                    await _userRoleRepository.AssignRoleToUserAsync(userId, role.Id, cancellationToken);
                }
            }
        }

        /// <summary>
        /// Carga los roles del usuario para incluir en la respuesta
        /// </summary>
        private async Task<List<RoleDropdownDto>> LoadUserRoles(Guid userId, CancellationToken cancellationToken)
        {
            var userRoles = await _userRoleRepository.GetUserRolesWithDetailsAsync(userId, cancellationToken);
            
            return userRoles
                .Where(ur => ur.Role != null && ur.Role.Status)
                .Select(ur => new RoleDropdownDto
                {
                    Id = ur.Role.Id,
                    Name = ur.Role.Name
                })
                .ToList();
        }
    }
}
