using Platform.Domain.DTOs.Auth;
using AutoMapper;
using Platform.Domain.Entities.Auth;
using Platform.Domain.Repositories;

namespace Platform.Application.Core.Auth.Queries.UserTypes
{
    public class GetUserTypesSummary
    {
        private readonly IRepositoryBase<UserType> _userTypeRepository;
        private readonly IMapper _mapper;

        public GetUserTypesSummary(IRepositoryBase<UserType> userTypeRepository, IMapper mapper)
        {
            _userTypeRepository = userTypeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserTypeSummaryDto>> HandleAsync(CancellationToken cancellationToken)
        {
            var userTypes = await _userTypeRepository.GetAll(cancellationToken);
            return _mapper.Map<IEnumerable<UserTypeSummaryDto>>(userTypes);
        }
    }
}
