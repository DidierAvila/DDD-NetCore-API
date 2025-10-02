using AutoMapper;
using Platform.Domain.Repositories;
using Platform.Domain.DTOs.Auth;
using Platform.Domain.Entities.Auth;

namespace Platform.Application.Core.Auth.Queries.UserTypes
{
    public class GetActiveUserTypes
    {
        private readonly IRepositoryBase<UserType> _userTypeRepository;
        private readonly IMapper _mapper;

        public GetActiveUserTypes(IRepositoryBase<UserType> userTypeRepository, IMapper mapper)
        {
            _userTypeRepository = userTypeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserTypeDto>> HandleAsync(CancellationToken cancellationToken)
        {
            var activeUserTypes = await _userTypeRepository.Finds(x => x.Status, cancellationToken);
            return _mapper.Map<IEnumerable<UserTypeDto>>(activeUserTypes);
        }
    }
}
