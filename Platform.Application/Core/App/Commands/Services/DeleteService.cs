using Platform.Domain.Repositories.App;

namespace Platform.Application.Core.App.Commands.Services
{
    public class DeleteService
    {
        private readonly IServiceRepository _serviceRepository;

        public DeleteService(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<bool> HandleAsync(Guid id, CancellationToken cancellationToken)
        {
            var service = await _serviceRepository.GetByID(id, cancellationToken);
            if (service == null)
                return false;

            await _serviceRepository.Delete(service, cancellationToken);
            return true;
        }
    }
}