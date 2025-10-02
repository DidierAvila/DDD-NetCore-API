using Platform.Domain.DTOs.App;
using Platform.Domain.DTOs.Common;

namespace Platform.Application.Core.App.Queries.Handlers
{
    public interface IServiceQueryHandler
    {
        Task<List<ServiceDto>> GetAllServicesAsync(CancellationToken cancellationToken = default);
        Task<ServiceDto?> GetServiceByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<ServiceDropdownDto>> GetActiveServicesAsync(CancellationToken cancellationToken = default);
        Task<PaginationResponseDto<ServiceSummaryDto>> GetServicesFilteredAsync(ServiceFilterDto filter, CancellationToken cancellationToken = default);
    }
}