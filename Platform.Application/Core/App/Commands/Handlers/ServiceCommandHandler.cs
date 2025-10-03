using Platform.Application.Core.App.Commands.Services;
using Platform.Domain.DTOs.App;

namespace Platform.Application.Core.App.Commands.Handlers
{
    public class ServiceCommandHandler : IServiceCommandHandler
    {
        private readonly CreateService _createService;
        private readonly UpdateService _updateService;
        private readonly DeleteService _deleteService;

        public ServiceCommandHandler(
            CreateService createService,
            UpdateService updateService,
            DeleteService deleteService)
        {
            _createService = createService;
            _updateService = updateService;
            _deleteService = deleteService;
        }

        public async Task<ServiceDto> CreateServiceAsync(CreateServiceDto createServiceDto, CancellationToken cancellationToken = default)
        {
            return await _createService.HandleAsync(createServiceDto, cancellationToken);
        }

        public async Task<ServiceDto?> UpdateServiceAsync(Guid id, UpdateServiceDto updateServiceDto, CancellationToken cancellationToken = default)
        {
            return await _updateService.HandleAsync(id, updateServiceDto, cancellationToken);
        }

        public async Task<bool> DeleteServiceAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _deleteService.HandleAsync(id, cancellationToken);
        }
    }
}