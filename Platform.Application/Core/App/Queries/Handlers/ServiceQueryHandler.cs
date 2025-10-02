using Platform.Application.Core.App.Queries.Services;
using Platform.Domain.DTOs.App;
using Platform.Domain.DTOs.Common;

namespace Platform.Application.Core.App.Queries.Handlers
{
    public class ServiceQueryHandler : IServiceQueryHandler
    {
        private readonly GetAllServices _getAllServices;
        private readonly GetServiceById _getServiceById;
        private readonly GetActiveServices _getActiveServices;
        private readonly GetServicesFiltered _getServicesFiltered;

        public ServiceQueryHandler(
            GetAllServices getAllServices,
            GetServiceById getServiceById,
            GetActiveServices getActiveServices,
            GetServicesFiltered getServicesFiltered)
        {
            _getAllServices = getAllServices;
            _getServiceById = getServiceById;
            _getActiveServices = getActiveServices;
            _getServicesFiltered = getServicesFiltered;
        }

        public async Task<List<ServiceDto>> GetAllServicesAsync(CancellationToken cancellationToken = default)
        {
            return await _getAllServices.HandleAsync(cancellationToken);
        }

        public async Task<ServiceDto?> GetServiceByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _getServiceById.HandleAsync(id, cancellationToken);
        }

        public async Task<List<ServiceDropdownDto>> GetActiveServicesAsync(CancellationToken cancellationToken = default)
        {
            return await _getActiveServices.HandleAsync(cancellationToken);
        }

        public async Task<PaginationResponseDto<ServiceSummaryDto>> GetServicesFilteredAsync(ServiceFilterDto filter, CancellationToken cancellationToken = default)
        {
            return await _getServicesFiltered.HandleAsync(filter, cancellationToken);
        }
    }
}