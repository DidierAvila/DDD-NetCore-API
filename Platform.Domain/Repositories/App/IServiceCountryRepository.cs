using Platform.Domain.Entities.App;

namespace Platform.Domain.Repositories.App
{
    public interface IServiceCountryRepository : IRepositoryBase<ServiceCountry>
    {
        Task<bool> AssignCountriesToServiceAsync(Guid serviceId, List<string> countryCodes);
        Task<bool> RemoveCountriesFromServiceAsync(Guid serviceId, List<string> countryCodes);
    }
}