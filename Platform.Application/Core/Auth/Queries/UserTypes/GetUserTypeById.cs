using AutoMapper;
using Platform.Domain.Repositories;
using Platform.Domain.DTOs.Auth;
using Platform.Domain.Entities.Auth;

namespace Platform.Application.Core.Auth.Queries.UserTypes
{
    public class GetUserTypeById
    {
        private readonly IRepositoryBase<UserType> _userTypeRepository;
        private readonly IMapper _mapper;

        public GetUserTypeById(IRepositoryBase<UserType> userTypeRepository, IMapper mapper)
        {
            _userTypeRepository = userTypeRepository;
            _mapper = mapper;
        }

        public async Task<UserTypeDto?> HandleAsync(Guid id, CancellationToken cancellationToken)
        {
            var userType = await _userTypeRepository.GetByID(id, cancellationToken);
            if (userType == null)
                return null;

            // Map Entity to DTO using AutoMapper
            return _mapper.Map<UserTypeDto>(userType);
        }
    }
}
