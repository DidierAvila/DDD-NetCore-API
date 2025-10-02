using AutoMapper;
using Platform.Domain.DTOs.App;
using Platform.Domain.Repositories.App;

namespace Platform.Application.Core.App.Queries.Services
{
    public class GetAllServices
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;

        public GetAllServices(IServiceRepository serviceRepository, IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
        }

        public async Task<List<ServiceDto>> HandleAsync(CancellationToken cancellationToken)
        {
            var services = await _serviceRepository.GetAll(cancellationToken);
            return _mapper.Map<List<ServiceDto>>(services);
        }
    }
}