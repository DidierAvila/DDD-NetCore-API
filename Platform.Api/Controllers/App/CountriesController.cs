using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Platform.Api.Attributes;
using Platform.Domain.DTOs.App;
using Platform.Domain.Repositories.App;
using Platform.Infrastructure.Services;

namespace Platform.Api.Controllers.App
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IServiceCountryRepository _serviceCountryRepository;
        private readonly CountrySyncService _countrySyncService;

        public CountriesController(
            ICountryRepository countryRepository,
            IServiceCountryRepository serviceCountryRepository,
            CountrySyncService countrySyncService)
        {
            _countryRepository = countryRepository;
            _serviceCountryRepository = serviceCountryRepository;
            _countrySyncService = countrySyncService;
        }

        [HttpGet]
        [RequirePermission("countries.read")]
        public async Task<ActionResult<IEnumerable<CountryDto>>> GetAllCountries()
        {
            var countries = await _countryRepository.GetAllAsync();
            var countryDtos = countries.Select(c => new CountryDto
            {
                Alpha2Code = c.Alpha2Code,
                Name = c.Name,
                Alpha3Code = c.Alpha3Code,
                Capital = c.Capital,
                Region = c.Region
            });

            return Ok(countryDtos);
        }

        [HttpGet("service/{serviceId}")]
        [RequirePermission("countries.read")]
        public async Task<ActionResult<IEnumerable<CountryDto>>> GetCountriesByService(Guid serviceId)
        {
            var countries = await _countryRepository.GetCountriesByServiceIdAsync(serviceId);
            var countryDtos = countries.Select(c => new CountryDto
            {
                Alpha2Code = c.Alpha2Code,
                Name = c.Name,
                Alpha3Code = c.Alpha3Code,
                Capital = c.Capital,
                Region = c.Region
            });

            return Ok(countryDtos);
        }

        [HttpPost("sync")]
        [RequirePermission("countries.sync")]
        public async Task<IActionResult> SyncCountries()
        {
            await _countrySyncService.SyncCountriesAsync();
            return Ok(new { message = "Sincronización de países iniciada" });
        }

        [HttpPost("service/assign")]
        [RequirePermission("countries.assign")]
        public async Task<IActionResult> AssignCountriesToService([FromBody] AssignCountryToServiceDto dto)
        {
            var result = await _serviceCountryRepository.AssignCountriesToServiceAsync(dto.ServiceId, dto.CountryCodes);
            
            if (result)
                return Ok(new { message = "Países asignados correctamente" });
            else
                return BadRequest(new { message = "Error al asignar países al servicio" });
        }

        [HttpPost("service/remove")]
        [RequirePermission("countries.remove")]
        public async Task<IActionResult> RemoveCountriesFromService([FromBody] AssignCountryToServiceDto dto)
        {
            var result = await _serviceCountryRepository.RemoveCountriesFromServiceAsync(dto.ServiceId, dto.CountryCodes);
            
            if (result)
                return Ok(new { message = "Países eliminados correctamente" });
            else
                return BadRequest(new { message = "Error al eliminar países del servicio" });
        }

        [HttpGet("dropdown")]
        [RequirePermission("countries.read")]
        [ProducesResponseType(typeof(IEnumerable<CountryDropdownDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<CountryDropdownDto>>> GetCountriesForDropdown(CancellationToken cancellationToken)
        {
            try
            {
                var countries = await _countryRepository.GetAllAsync(cancellationToken);
                var dropdownItems = countries
                    .OrderBy(c => c.Name)
                    .Select(c => new CountryDropdownDto
                    {
                        Value = c.Alpha2Code,
                        Label = c.Name
                    })
                    .ToList();

                return Ok(dropdownItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener la lista de países para el dropdown", error = ex.Message });
            }
        }
    }
}