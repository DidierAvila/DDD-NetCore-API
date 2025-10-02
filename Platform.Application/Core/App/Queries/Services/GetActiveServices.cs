using AutoMapper;
using Platform.Domain.DTOs.App;
using Platform.Domain.Repositories.App;

namespace Platform.Application.Core.App.Queries.Services
{
    public class GetActiveServices
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;

        public GetActiveServices(IServiceRepository serviceRepository, IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
        }

        public async Task<List<ServiceDropdownDto>> HandleAsync(CancellationToken cancellationToken)
        {
            var services = await _serviceRepository.GetActiveServicesAsync(cancellationToken);
            return _mapper.Map<List<ServiceDropdownDto>>(services);
        }
    }
}