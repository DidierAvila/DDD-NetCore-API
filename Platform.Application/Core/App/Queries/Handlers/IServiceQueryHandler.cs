using Platform.Domain.DTOs.App;
using Platform.Domain.DTOs.Common;

namespace Platform.Application.Core.App.Queries.Handlers
{
    public interface IServiceQueryHandler
    {
        Task<ServiceDto?> GetServiceByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<ServiceDropdownDto>> GetActiveServicesAsync(CancellationToken cancellationToken = default);
        Task<PaginationResponseDto<ServiceSummaryDto>> GetAllServicesWithPaginationAsync(ServiceFilterDto filter, CancellationToken cancellationToken = default);
    }
}