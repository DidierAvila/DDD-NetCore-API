using AutoMapper;
using Platform.Domain.DTOs.App;
using Platform.Domain.DTOs.Common;
using Platform.Domain.Repositories.App;
using System;
using System.Linq;

namespace Platform.Application.Core.App.Queries.Services
{
    public class GetAllServices
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;

        public GetAllServices(IServiceRepository serviceRepository, IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
        }

        public async Task<List<ServiceDto>> HandleAsync(CancellationToken cancellationToken)
        {
            var services = await _serviceRepository.GetAllWithSupplierAsync(cancellationToken);
            return _mapper.Map<List<ServiceDto>>(services);
        }

        public async Task<PaginationResponseDto<ServiceSummaryDto>> HandleWithPaginationAsync(ServiceFilterDto filter, CancellationToken cancellationToken)
        {
            // Validar y establecer valores por defecto
            if (filter.Page <= 0) filter.Page = 1;
            if (filter.PageSize <= 0) filter.PageSize = 10;
            if (filter.PageSize > 100) filter.PageSize = 100;

            // Obtener todos los servicios con información del proveedor
            var allServices = await _serviceRepository.GetAllWithSupplierAsync(cancellationToken);
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

            // Contar total de registros
            var totalRecords = query.Count();

            // Aplicar ordenamiento si se especifica
            if (!string.IsNullOrEmpty(filter.SortBy))
            {
                query = ApplySorting(query, filter.SortBy);
            }

            // Aplicar paginación
            var services = query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            // Mapear a DTOs
            var serviceDtos = _mapper.Map<List<ServiceSummaryDto>>(services);

            return new PaginationResponseDto<ServiceSummaryDto>
            {
                Data = serviceDtos,
                TotalRecords = totalRecords,
                CurrentPage = filter.Page,
                PageSize = filter.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalRecords / filter.PageSize)
            };
        }

        private IQueryable<Domain.Entities.App.Service> ApplySorting(IQueryable<Domain.Entities.App.Service> query, string sortBy)
        {
            sortBy = sortBy.ToLower();
            
            return sortBy switch
            {
                "name" => query.OrderBy(s => s.Name),
                "name_desc" => query.OrderByDescending(s => s.Name),
                "createdat" => query.OrderBy(s => s.CreatedAt),
                "createdat_desc" => query.OrderByDescending(s => s.CreatedAt),
                "status" => query.OrderBy(s => s.Status),
                "status_desc" => query.OrderByDescending(s => s.Status),
                _ => query.OrderBy(s => s.Name)
            };
        }
    }
}