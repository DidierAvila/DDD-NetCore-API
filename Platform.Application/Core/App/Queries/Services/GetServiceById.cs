using AutoMapper;
using Platform.Domain.DTOs.App;
using Platform.Domain.Repositories.App;

namespace Platform.Application.Core.App.Queries.Services
{
    public class GetServiceById
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public GetServiceById(
            IServiceRepository serviceRepository, 
            ICountryRepository countryRepository,
            IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        public async Task<ServiceDto?> HandleAsync(Guid id, CancellationToken cancellationToken)
        {
            var service = await _serviceRepository.GetByIdWithSupplierAsync(id, cancellationToken);
            if (service == null)
                return null;
                
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