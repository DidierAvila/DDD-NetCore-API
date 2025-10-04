using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Platform.Api.Attributes;
using Platform.Application.Core.App.Queries.Reports;
using Platform.Domain.DTOs.App.Reports;

namespace Platform.Api.Controllers.App
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly IReportQueryHandler _reportQueryHandler;

        public ReportsController(IReportQueryHandler reportQueryHandler)
        {
            _reportQueryHandler = reportQueryHandler;
        }

        /// <summary>
        /// Obtiene el resumen de usuarios por tipo para visualización en gráficos
        /// </summary>
        [HttpGet("users-by-type")]
        [RequirePermission("reports.read")]
        public async Task<ActionResult<UsersByTypeReportDto>> GetUsersByTypeSummary(
            [FromQuery] UsersByTypeReportRequestDto requestDto,
            CancellationToken cancellationToken)
        {
            var result = await _reportQueryHandler.GetUsersByTypeReport(requestDto, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Exporta el reporte de usuarios por tipo a Excel
        /// </summary>
        [HttpGet("users-by-type/export")]
        [RequirePermission("reports.export")]
        public async Task<IActionResult> ExportUsersByTypeReport(
            [FromQuery] UsersByTypeReportRequestDto requestDto,
            CancellationToken cancellationToken)
        {
            var fileContent = await _reportQueryHandler.ExportUsersByTypeReport(requestDto, cancellationToken);
            return File(
                fileContent,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "ReporteUsuariosPorTipo.xlsx");
        }
    }
}