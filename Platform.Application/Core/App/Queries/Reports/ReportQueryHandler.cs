using OfficeOpenXml;
using Platform.Domain.DTOs.App.Reports;
using Platform.Domain.Entities.Auth;
using Platform.Domain.Repositories;

namespace Platform.Application.Core.App.Queries.Reports
{
    public class ReportQueryHandler : IReportQueryHandler
    {
        private readonly IRepositoryBase<User> _userRepository;
        private readonly IRepositoryBase<UserType> _userTypeRepository;

        // Colores predefinidos para gráficos
        private readonly List<string> _chartColors =
        [
            "#4e73df", "#1cc88a", "#36b9cc", "#f6c23e", "#e74a3b",
            "#6f42c1", "#5a5c69", "#858796", "#f8f9fc", "#d1d3e2"
        ];

        public ReportQueryHandler(
            IRepositoryBase<User> userRepository,
            IRepositoryBase<UserType> userTypeRepository)
        {
            _userRepository = userRepository;
            _userTypeRepository = userTypeRepository;
        }

        public async Task<UsersByTypeReportDto> GetUsersByTypeReport(UsersByTypeReportRequestDto request, CancellationToken cancellationToken)
        {
            // Obtener todos los usuarios y tipos de usuario
            var users = await _userRepository.GetAll(cancellationToken);
            var userTypes = await _userTypeRepository.GetAll(cancellationToken);
            
            // Aplicar filtros
            var filteredUsers = users.AsEnumerable();
            
            if (request.StartDate.HasValue)
            {
                filteredUsers = filteredUsers.Where(u => u.CreatedAt >= request.StartDate.Value);
            }

            if (request.EndDate.HasValue)
            {
                var endDate = request.EndDate.Value.AddDays(1).AddSeconds(-1); // Hasta el final del día
                filteredUsers = filteredUsers.Where(u => u.CreatedAt <= endDate);
            }

            if (request.UserTypeId.HasValue)
            {
                filteredUsers = filteredUsers.Where(u => u.UserTypeId == request.UserTypeId.Value);
            }

            if (!request.IncludeInactive)
             {
                 filteredUsers = filteredUsers.Where(u => u.Status);
            }

            // Convertir a lista y contar
            var filteredUsersList = filteredUsers.ToList();
            var totalUsers = filteredUsersList.Count;

            // Agrupar por tipo de usuario
            var usersByType = filteredUsersList
                .GroupBy(u => u.UserTypeId)
                .Select(g => new
                {
                    TypeId = g.Key,
                    TypeName = userTypes.FirstOrDefault(ut => ut.Id == g.Key)?.Name ?? "Sin tipo",
                    Users = g.ToList(),
                    Count = g.Count()
                })
                .OrderByDescending(g => g.Count)
                .ToList();

            // Crear respuesta
            var result = new UsersByTypeReportDto
            {
                TotalUsers = totalUsers,
                GeneratedAt = DateTime.Now
            };

            // Preparar datos para gráficos y detalles
            int colorIndex = 0;
            foreach (var group in usersByType)
            {
                var percentage = totalUsers > 0 ? (decimal)group.Count / totalUsers * 100 : 0;
                var color = _chartColors[colorIndex % _chartColors.Count];
                colorIndex++;

                // Datos para gráfico circular
                result.PieChartData.Add(new ChartDataItem
                {
                    Label = group.TypeName,
                    Value = group.Count,
                    Percentage = Math.Round(percentage, 2),
                    Color = color
                });

                // Datos para gráfico de barras
                result.Labels.Add(group.TypeName);
                result.Values.Add(group.Count);

                // Datos detallados
                var detailItem = new UserTypeReportItem
                {
                    Id = group.TypeId,
                    Name = group.TypeName,
                    UserCount = group.Count,
                    Percentage = Math.Round(percentage, 2),
                    Users = group.Users.Select(u => new UserReportSummary
                    {
                        Id = u.Id,
                        Email = u.Email,
                        Name = u.Name,
                        CreatedAt = u.CreatedAt ?? DateTime.MinValue,
                        LastLoginAt = u.CreatedAt
                    }).ToList()
                };

                result.Details.Add(detailItem);
            }

            return result;
        }

        public async Task<byte[]> ExportUsersByTypeReport(UsersByTypeReportRequestDto requestDto, CancellationToken cancellationToken)
        {
            // Obtener datos del reporte
            var reportData = await GetUsersByTypeReport(requestDto, cancellationToken);

            // Crear archivo Excel simplificado
            using var package = new ExcelPackage();
            
            // Crear hoja de resumen
            var summarySheet = package.Workbook.Worksheets.Add("Resumen");
            
            // Encabezado
            summarySheet.Cells[1, 1].Value = "REPORTE DE USUARIOS POR TIPO";
            summarySheet.Cells[1, 1].Style.Font.Bold = true;
            summarySheet.Cells[1, 1].Style.Font.Size = 14;
            
            summarySheet.Cells[2, 1].Value = $"Fecha de generación: {reportData.GeneratedAt:dd/MM/yyyy HH:mm}";
            summarySheet.Cells[3, 1].Value = $"Total de usuarios: {reportData.TotalUsers}";
            
            // Tabla de resumen
            summarySheet.Cells[5, 1].Value = "Tipo de Usuario";
            summarySheet.Cells[5, 2].Value = "Cantidad";
            summarySheet.Cells[5, 3].Value = "Porcentaje";
            
            int row = 6;
            foreach (var item in reportData.Details)
            {
                summarySheet.Cells[row, 1].Value = item.Name;
                summarySheet.Cells[row, 2].Value = item.UserCount;
                summarySheet.Cells[row, 3].Value = $"{item.Percentage}%";
                row++;
            }
            
            // Crear hoja de detalles
            var detailsSheet = package.Workbook.Worksheets.Add("Detalles");
            
            // Encabezado de detalles
            detailsSheet.Cells[1, 1].Value = "Tipo de Usuario";
            detailsSheet.Cells[1, 2].Value = "Email";
            detailsSheet.Cells[1, 3].Value = "Nombre";
            detailsSheet.Cells[1, 4].Value = "Fecha de Creación";
            
            row = 2;
            foreach (var typeItem in reportData.Details)
            {
                if (typeItem.Users != null)
                {
                    foreach (var user in typeItem.Users)
                    {
                        detailsSheet.Cells[row, 1].Value = typeItem.Name;
                        detailsSheet.Cells[row, 2].Value = user.Email;
                        detailsSheet.Cells[row, 3].Value = user.Name;
                        detailsSheet.Cells[row, 4].Value = user.CreatedAt.ToString("dd/MM/yyyy HH:mm");
                        row++;
                    }
                }
            }
            
            // Convertir a array de bytes
            return package.GetAsByteArray();
        }
    }
}