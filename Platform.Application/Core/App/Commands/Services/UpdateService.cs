using AutoMapper;
using Platform.Domain.DTOs.App;
using Platform.Domain.Entities.App;
using Platform.Domain.Repositories.App;

namespace Platform.Application.Core.App.Commands.Services
{
    public class UpdateService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;

        public UpdateService(IServiceRepository serviceRepository, IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
        }

        public async Task<ServiceDto?> HandleAsync(Guid id, UpdateServiceDto updateServiceDto, CancellationToken cancellationToken)
        {
            var service = await _serviceRepository.GetByID(id, cancellationToken);
            if (service == null)
                return null;

            // Verificar si ya existe otro servicio con el mismo nombre
            var existingServices = await _serviceRepository.GetAll(cancellationToken);
            if (existingServices.Any(s => s.Id != id && s.Name.Equals(updateServiceDto.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"Ya existe otro servicio con el nombre '{updateServiceDto.Name}'");
            }

            // Verificar si ya existe otro servicio con el mismo endpoint y método
            var existingEndpoint = await _serviceRepository.GetByEndpointAsync(updateServiceDto.Endpoint, cancellationToken);
            if (existingEndpoint != null && existingEndpoint.Id != id && existingEndpoint.Method.Equals(updateServiceDto.Method, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException($"Ya existe otro servicio con el endpoint '{updateServiceDto.Endpoint}' y método '{updateServiceDto.Method}'");
            }

            // Actualizar las propiedades
            _mapper.Map(updateServiceDto, service);
            service.UpdatedAt = DateTime.UtcNow;

            await _serviceRepository.Update(service, cancellationToken);
            return _mapper.Map<ServiceDto>(service);
        }
    }
}