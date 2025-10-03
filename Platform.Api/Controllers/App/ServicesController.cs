using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Platform.Api.Attributes;
using Platform.Application.Core.App.Commands.Handlers;
using Platform.Application.Core.App.Queries.Handlers;
using Platform.Application.Utils;
using Platform.Domain.DTOs.App;
using Platform.Domain.DTOs.Common;

namespace Platform.Api.Controllers.App
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ServicesController : ControllerBase
    {
        private readonly IServiceCommandHandler _serviceCommandHandler;
        private readonly IServiceQueryHandler _serviceQueryHandler;

        public ServicesController(
            IServiceCommandHandler serviceCommandHandler,
            IServiceQueryHandler serviceQueryHandler)
        {
            _serviceCommandHandler = serviceCommandHandler;
            _serviceQueryHandler = serviceQueryHandler;
        }

        /// <summary>
        /// Obtiene todos los servicios con paginación y filtros
        /// </summary>
        /// <param name="filter">Filtros de búsqueda y paginación</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Lista paginada de servicios</returns>
        /// <remarks>
        /// Campos disponibles para SortBy: name, createdat, status
        /// 
        /// Ejemplo de uso:
        /// GET /api/services?page=1&amp;pageSize=10&amp;name=servicio&amp;status=true&amp;sortBy=name
        /// </remarks>
        [HttpGet]
        [RequirePermission("services.read")]
        public async Task<ActionResult<PaginationResponseDto<ServiceSummaryDto>>> GetAllServices(
            [FromQuery] ServiceFilterDto filter, 
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await _serviceQueryHandler.GetAllServicesWithPaginationAsync(filter, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un servicio por ID
        /// </summary>
        [HttpGet("{id}")]
        [RequirePermission("services.read")]
        public async Task<ActionResult<ServiceDto>> GetServiceById(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var service = await _serviceQueryHandler.GetServiceByIdAsync(id, cancellationToken);
                if (service == null)
                    return NotFound(new { message = "Servicio no encontrado" });

                return Ok(service);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        /// <summary>
        /// Obtiene servicios activos para dropdown
        /// </summary>
        [HttpGet("dropdown")]
        [RequirePermission("services.read")]
        public async Task<ActionResult<List<ServiceDropdownDto>>> GetActiveServices(CancellationToken cancellationToken)
        {
            try
            {
                var services = await _serviceQueryHandler.GetActiveServicesAsync(cancellationToken);
                return Ok(services);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo servicio
        /// </summary>
        [HttpPost]
        [RequirePermission("services.create")]
        public async Task<ActionResult<ServiceDto>> CreateService([FromBody] CreateServiceDto createServiceDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (createServiceDto != null && createServiceDto.SupplierId == Guid.Empty)
                {
                    // Obtener el ID del usuario desde el token JWT
                    var userIdClaim = User.FindFirst(CustomClaimTypes.UserId)?.Value;
                    if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                    {
                        return Unauthorized(new { message = "Invalid or missing user ID in token" });
                    }
                    createServiceDto.SupplierId = userId;
                }
                var service = await _serviceCommandHandler.CreateServiceAsync(createServiceDto!, cancellationToken);
                return CreatedAtAction(nameof(GetServiceById), new { id = service.Id }, service);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza un servicio existente
        /// </summary>
        [HttpPut("{id}")]
        [RequirePermission("services.update")]
        public async Task<ActionResult<ServiceDto>> UpdateService(Guid id, [FromBody] UpdateServiceDto updateServiceDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var service = await _serviceCommandHandler.UpdateServiceAsync(id, updateServiceDto, cancellationToken);
                if (service == null)
                    return NotFound(new { message = "Servicio no encontrado" });

                return Ok(service);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Elimina un servicio
        /// </summary>
        [HttpDelete("{id}")]
        [RequirePermission("services.delete")]
        public async Task<ActionResult> DeleteService(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var deleted = await _serviceCommandHandler.DeleteServiceAsync(id, cancellationToken);
                if (!deleted)
                    return NotFound(new { message = "Servicio no encontrado" });

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}