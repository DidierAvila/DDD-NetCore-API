using AutoMapper;
using Platform.Domain.Repositories;
using Platform.Domain.DTOs.Auth;
using Platform.Domain.Entities.Auth;

namespace Platform.Application.Core.Auth.Queries.UserTypes
{
    public class GetAllUserTypes
    {
        private readonly IRepositoryBase<UserType> _userTypeRepository;
        private readonly IMapper _mapper;

        public GetAllUserTypes(IRepositoryBase<UserType> userTypeRepository, IMapper mapper)
        {
            _userTypeRepository = userTypeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserTypeDto>> HandleAsync(CancellationToken cancellationToken)
        {
            var userTypes = await _userTypeRepository.GetAll(cancellationToken);
            return _mapper.Map<IEnumerable<UserTypeDto>>(userTypes);
        }
    }
}
