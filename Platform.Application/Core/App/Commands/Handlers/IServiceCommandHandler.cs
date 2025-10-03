using Platform.Domain.DTOs.App;

namespace Platform.Application.Core.App.Commands.Handlers
{
    public interface IServiceCommandHandler
    {
        Task<ServiceDto> CreateServiceAsync(CreateServiceDto createServiceDto, CancellationToken cancellationToken = default);
        Task<ServiceDto?> UpdateServiceAsync(Guid id, UpdateServiceDto updateServiceDto, CancellationToken cancellationToken = default);
        Task<bool> DeleteServiceAsync(Guid id, CancellationToken cancellationToken = default);
    }
}