using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Platform.Domain.Entities.App;
using Platform.Infrastructure.DbContexts;

namespace Platform.Infrastructure.Services
{
    public class CountrySyncService
    {
        private readonly PlatformDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CountrySyncService> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _countriesApiUrl;

        public CountrySyncService(
            PlatformDbContext context,
            IHttpClientFactory httpClientFactory,
            ILogger<CountrySyncService> logger,
            IConfiguration configuration)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _configuration = configuration;
            _countriesApiUrl = _configuration["ExternalServices:CountriesApi:Url"] ?? 
                throw new InvalidOperationException("La URL de la API de países no está configurada en appsettings.json");
        }

        public async Task SyncCountriesAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync(_countriesApiUrl);
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var countries = JsonSerializer.Deserialize<List<CountryApiModel>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (countries != null)
                    {
                        await UpdateCountriesInDatabase(countries);
                    }
                }
                else
                {
                    _logger.LogError($"Error al obtener países: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al sincronizar países");
            }
        }

        private async Task UpdateCountriesInDatabase(List<CountryApiModel> countries)
        {
            var existingCountries = await _context.Countries.ToListAsync();
            var existingCountryCodes = existingCountries.Select(c => c.Alpha2Code).ToHashSet();

            // Agregar nuevos países
            foreach (var countryModel in countries)
            {
                if (!existingCountryCodes.Contains(countryModel.Alpha2Code))
                {
                    var country = new Country
                    {
                        Alpha2Code = countryModel.Alpha2Code,
                        Name = countryModel.Name,
                        Alpha3Code = countryModel.Alpha3Code,
                        Capital = countryModel.Capital,
                        Region = countryModel.Region
                    };

                    _context.Countries.Add(country);
                }
                else
                {
                    // Actualizar países existentes
                    var existingCountry = existingCountries.First(c => c.Alpha2Code == countryModel.Alpha2Code);
                    existingCountry.Name = countryModel.Name;
                    existingCountry.Alpha3Code = countryModel.Alpha3Code;
                    existingCountry.Capital = countryModel.Capital;
                    existingCountry.Region = countryModel.Region;
                }
            }

            await _context.SaveChangesAsync();
        }
    }

    public class CountryApiModel
    {
        public string Name { get; set; } = null!;
        public string Alpha2Code { get; set; } = null!;
        public string? Alpha3Code { get; set; }
        public string? Capital { get; set; }
        public string? Region { get; set; }
    }
}