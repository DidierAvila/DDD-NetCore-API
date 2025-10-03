using AutoMapper;
using Platform.Domain.DTOs.App;
using Platform.Domain.Repositories.App;

namespace Platform.Application.Core.App.Commands.Services
{
    public class UpdateService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IServiceCountryRepository _serviceCountryRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public UpdateService(
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

        public async Task<ServiceDto?> HandleAsync(Guid id, UpdateServiceDto updateServiceDto, CancellationToken cancellationToken)
        {
            var service = await _serviceRepository.GetByID(id, cancellationToken);
            if (service == null)
                return null;

            // Verificar si ya existe otro servicio con el mismo nombre
            if (await _serviceRepository.ExistsByNameAsync(updateServiceDto.Name, id))
            {
                throw new InvalidOperationException($"Ya existe otro servicio con el nombre '{updateServiceDto.Name}'");
            }

            // Actualizar las propiedades
            _mapper.Map(updateServiceDto, service);
            service.UpdatedAt = DateTime.UtcNow;

            await _serviceRepository.Update(service, cancellationToken);
            
            // Actualizar los países asociados si se proporcionaron
            if (updateServiceDto.CountryCodes != null)
            {
                // Obtener los países actuales para eliminarlos
                var currentCountries = await _countryRepository.GetCountriesByServiceIdAsync(id, cancellationToken);
                var currentCountryCodes = currentCountries.Select(c => c.Alpha2Code).ToList();
                
                // Eliminar los países actuales
                if (currentCountryCodes.Count > 0)
                {
                    await _serviceCountryRepository.RemoveCountriesFromServiceAsync(id, currentCountryCodes);
                }
                
                // Luego agregamos las nuevas asociaciones
                if (updateServiceDto.CountryCodes.Count > 0)
                {
                    await _serviceCountryRepository.AssignCountriesToServiceAsync(id, updateServiceDto.CountryCodes);
                }
            }
            
            // Obtener el servicio actualizado con sus países
            var serviceDto = _mapper.Map<ServiceDto>(service);
            
            // Obtener los países asociados al servicio
            var countries = await _countryRepository.GetCountriesByServiceIdAsync(id, cancellationToken);
            
            // Mapear los países a ServiceCountryDto
            serviceDto.Countries = countries.Select(c => new ServiceCountryDto
            {
                ServiceId = id,
                CountryCode = c.Alpha2Code,
                CountryName = c.Name
            }).ToList();
            
            return serviceDto;
        }
    }
}