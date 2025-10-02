using AutoMapper;
using Platform.Domain.Repositories;
using Platform.Domain.DTOs.Auth;
using Platform.Domain.Entities.Auth;

namespace Platform.Application.Core.Auth.Commands.UserTypes
{
    public class CreateUserType
    {
        private readonly IRepositoryBase<UserType> _userTypeRepository;
        private readonly IMapper _mapper;

        public CreateUserType(IRepositoryBase<UserType> userTypeRepository, IMapper mapper)
        {
            _userTypeRepository = userTypeRepository;
            _mapper = mapper;
        }

        public async Task<UserTypeDto> HandleAsync(CreateUserTypeDto createUserTypeDto, CancellationToken cancellationToken)
        {
            // Validate that the name doesn't already exist
            var existingUserType = await _userTypeRepository.Find(x => x.Name == createUserTypeDto.Name, cancellationToken);
            if (existingUserType != null)
                throw new InvalidOperationException("A UserType with this name already exists");

            // Map DTO to Entity using AutoMapper
            var userType = _mapper.Map<UserType>(createUserTypeDto);

            // Create the UserType
            await _userTypeRepository.Create(userType, cancellationToken);

            // Map Entity back to DTO for return
            return _mapper.Map<UserTypeDto>(userType);
        }
    }
}
