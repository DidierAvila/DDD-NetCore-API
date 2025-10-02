using AutoMapper;
using Platform.Domain.Repositories;
using Platform.Domain.DTOs.Auth;
using Platform.Domain.Entities.Auth;

namespace Platform.Application.Core.Auth.Queries.Users
{
    public class GetAllUsers
    {
        private readonly IRepositoryBase<User> _userRepository;
        private readonly IRepositoryBase<UserType> _userTypeRepository;
        private readonly IMapper _mapper;

        public GetAllUsers(IRepositoryBase<User> userRepository, IRepositoryBase<UserType> userTypeRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _userTypeRepository = userTypeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> HandleAsync(CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAll(cancellationToken);
            var userTypes = await _userTypeRepository.GetAll(cancellationToken);

            // Map collection of Entities to DTOs using AutoMapper
            var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);

            // Asignar UserTypeName manualmente
            foreach (var userDto in userDtos)
            {
                var userType = userTypes.FirstOrDefault(ut => ut.Id == userDto.UserTypeId);
                userDto.UserTypeName = userType?.Name;
            }

            return userDtos;
        }
    }
}
