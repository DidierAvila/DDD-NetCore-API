using AutoMapper;
using Platform.Domain.DTOs.App;
using Platform.Domain.Repositories.App;

namespace Platform.Application.Core.App.Queries.Services
{
    public class GetServiceById
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;

        public GetServiceById(IServiceRepository serviceRepository, IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
        }

        public async Task<ServiceDto?> HandleAsync(Guid id, CancellationToken cancellationToken)
        {
            var service = await _serviceRepository.GetByID(id, cancellationToken);
            return service != null ? _mapper.Map<ServiceDto>(service) : null;
        }
    }
}