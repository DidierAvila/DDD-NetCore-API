using AutoMapper;
using Platform.Domain.DTOs.App;
using Platform.Domain.Entities.App;
using Platform.Domain.Repositories.App;

namespace Platform.Application.Core.App.Commands.Services
{
    public class CreateService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IServiceCountryRepository _serviceCountryRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CreateService(
            IServiceRepository serviceRepository, 
            IServiceCountryRepository serviceCountryRepository,
            ICountryRepository countryRepository,
            IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _serviceCountryRepository = serviceCountryRepository;
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        public async Task<ServiceDto> HandleAsync(CreateServiceDto createServiceDto, CancellationToken cancellationToken)
        {
            // Verificar si ya existe un servicio con el mismo nombre
            if (await _serviceRepository.ExistsByNameAsync(createServiceDto.Name))
            {
                throw new InvalidOperationException($"Ya existe un servicio con el nombre '{createServiceDto.Name}'");
            }

            // Mapear y crear el servicio
            var service = _mapper.Map<Service>(createServiceDto);
            service.Id = Guid.NewGuid();
            service.CreatedAt = DateTime.UtcNow;
            service.UpdatedAt = DateTime.UtcNow;

            await _serviceRepository.Create(service, cancellationToken);
            
            // Asociar países si se proporcionaron
            if (createServiceDto.CountryCodes != null && createServiceDto.CountryCodes.Count > 0)
            {
                await _serviceCountryRepository.AssignCountriesToServiceAsync(service.Id, createServiceDto.CountryCodes);
            }
            
            // Obtener el servicio con sus países
            var serviceDto = _mapper.Map<ServiceDto>(service);
            
            // Obtener los países asociados al servicio
            if (createServiceDto.CountryCodes != null && createServiceDto.CountryCodes.Count > 0)
            {
                var countries = await _countryRepository.GetCountriesByServiceIdAsync(service.Id, cancellationToken);
                
                // Mapear los países a ServiceCountryDto
                serviceDto.Countries = countries.Select(c => new ServiceCountryDto
                {
                    ServiceId = service.Id,
                    CountryCode = c.Alpha2Code,
                    CountryName = c.Name
                }).ToList();
            }
            
            return serviceDto;
        }
    }
}