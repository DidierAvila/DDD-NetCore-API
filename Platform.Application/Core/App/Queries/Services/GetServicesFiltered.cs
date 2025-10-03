using AutoMapper;
using Platform.Domain.DTOs.App;
using Platform.Domain.DTOs.Common;
using Platform.Domain.Repositories.App;

namespace Platform.Application.Core.App.Queries.Services
{
    public class GetServicesFiltered
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;

        public GetServicesFiltered(IServiceRepository serviceRepository, IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
        }

        public async Task<PaginationResponseDto<ServiceSummaryDto>> HandleAsync(ServiceFilterDto filter, CancellationToken cancellationToken)
        {
            // Obtener todos los servicios y aplicar filtros en memoria por ahora
            var allServices = await _serviceRepository.GetAll(cancellationToken);
            var query = allServices.AsQueryable();

            // Aplicar filtros
            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(s => s.Name.Contains(filter.Name, StringComparison.OrdinalIgnoreCase));
            }

            if (filter.Status.HasValue)
            {
                query = query.Where(s => s.Status == filter.Status.Value);
            }

            // Aplicar paginación
            var totalCount = query.Count();
            var services = query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            var serviceDtos = _mapper.Map<List<ServiceSummaryDto>>(services);

            return new PaginationResponseDto<ServiceSummaryDto>
            {
                Data = serviceDtos,
                TotalRecords = totalCount,
                CurrentPage = filter.Page,
                PageSize = filter.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / filter.PageSize)
            };
        }
    }
}