using AutoMapper;
using Platform.Domain.Repositories;
using Platform.Domain.DTOs.Auth;
using Platform.Domain.Entities.Auth;

namespace Platform.Application.Core.Auth.Commands.UserTypes
{
    public class UpdateUserType
    {
        private readonly IRepositoryBase<UserType> _userTypeRepository;
        private readonly IMapper _mapper;

        public UpdateUserType(IRepositoryBase<UserType> userTypeRepository, IMapper mapper)
        {
            _userTypeRepository = userTypeRepository;
            _mapper = mapper;
        }

        public async Task<UserTypeDto> HandleAsync(Guid id, UpdateUserTypeDto updateUserTypeDto, CancellationToken cancellationToken)
        {
            // Find existing userType
            var userType = await _userTypeRepository.Find(x => x.Id == id, cancellationToken);
            if (userType == null)
                throw new KeyNotFoundException("UserType not found");

            // Validate that the name doesn't already exist (if it's being updated)
            if (!string.IsNullOrWhiteSpace(updateUserTypeDto.Name) && 
                updateUserTypeDto.Name != userType.Name)
            {
                var existingUserType = await _userTypeRepository.Find(x => x.Name == updateUserTypeDto.Name, cancellationToken);
                if (existingUserType != null)
                    throw new InvalidOperationException("A UserType with this name already exists");
            }

            // Map updated values from DTO to existing entity
            _mapper.Map(updateUserTypeDto, userType);

            // Update the UserType
            await _userTypeRepository.Update(userType, cancellationToken);

            // Map Entity back to DTO for return
            return _mapper.Map<UserTypeDto>(userType);
        }
    }
}
