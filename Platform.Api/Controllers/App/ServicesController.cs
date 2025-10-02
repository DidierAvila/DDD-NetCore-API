using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Platform.Application.Core.App.Commands.Handlers;
using Platform.Application.Core.App.Queries.Handlers;
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
        /// Obtiene todos los servicios
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<ServiceDto>>> GetAllServices(CancellationToken cancellationToken)
        {
            try
            {
                var services = await _serviceQueryHandler.GetAllServicesAsync(cancellationToken);
                return Ok(services);
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
        /// Obtiene servicios filtrados con paginación
        /// </summary>
        [HttpPost("filtered")]
        public async Task<ActionResult<PaginationResponseDto<ServiceSummaryDto>>> GetServicesFiltered(
            [FromBody] ServiceFilterDto filter, 
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await _serviceQueryHandler.GetServicesFilteredAsync(filter, cancellationToken);
                return Ok(result);
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
        public async Task<ActionResult<ServiceDto>> CreateService([FromBody] CreateServiceDto createServiceDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var service = await _serviceCommandHandler.CreateServiceAsync(createServiceDto, cancellationToken);
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