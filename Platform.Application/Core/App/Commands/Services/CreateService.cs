using AutoMapper;
using Platform.Domain.DTOs.App;
using Platform.Domain.Entities.App;
using Platform.Domain.Repositories.App;

namespace Platform.Application.Core.App.Commands.Services
{
    public class CreateService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;

        public CreateService(IServiceRepository serviceRepository, IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
        }

        public async Task<ServiceDto> HandleAsync(CreateServiceDto createServiceDto, CancellationToken cancellationToken)
        {
            // Verificar si ya existe un servicio con el mismo nombre
            var existingServices = await _serviceRepository.GetAll(cancellationToken);
            if (existingServices.Any(s => s.Name.Equals(createServiceDto.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"Ya existe un servicio con el nombre '{createServiceDto.Name}'");
            }

            // Verificar si ya existe un servicio con el mismo endpoint y método
            var existingEndpoint = await _serviceRepository.GetByEndpointAsync(createServiceDto.Endpoint, cancellationToken);
            if (existingEndpoint != null && existingEndpoint.Method.Equals(createServiceDto.Method, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException($"Ya existe un servicio con el endpoint '{createServiceDto.Endpoint}' y método '{createServiceDto.Method}'");
            }

            // Mapear y crear el servicio
            var service = _mapper.Map<Service>(createServiceDto);
            service.Id = Guid.NewGuid();
            service.CreatedAt = DateTime.UtcNow;
            service.UpdatedAt = DateTime.UtcNow;

            await _serviceRepository.Create(service, cancellationToken);
            
            return _mapper.Map<ServiceDto>(service);
        }
    }
}