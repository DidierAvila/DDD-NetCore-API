namespace Platform.Domain.DTOs.App.Reports
{
    /// <summary>
    /// DTO para solicitud de reporte de usuarios por tipo
    /// </summary>
    public class UsersByTypeReportRequestDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid? UserTypeId { get; set; }
        public bool IncludeInactive { get; set; } = false;
    }

    /// <summary>
    /// DTO para respuesta de reporte de usuarios por tipo
    /// </summary>
    public class UsersByTypeReportDto
    {
        // Datos para gráfico circular (pie chart)
        public List<ChartDataItem> PieChartData { get; set; } = new();
        
        // Datos para gráfico de barras (bar chart)
        public List<string> Labels { get; set; } = new();
        public List<int> Values { get; set; } = new();
        
        // Datos detallados para tabla
        public List<UserTypeReportItem> Details { get; set; } = new();
        
        public int TotalUsers { get; set; }
        public DateTime GeneratedAt { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// DTO para elemento de datos de gráfico
    /// </summary>
    public class ChartDataItem
    {
        public string Label { get; set; } = null!;
        public int Value { get; set; }
        public decimal Percentage { get; set; }
        public string? Color { get; set; }
    }

    /// <summary>
    /// DTO para elemento de reporte de tipo de usuario
    /// </summary>
    public class UserTypeReportItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int UserCount { get; set; }
        public decimal Percentage { get; set; }
        public List<UserReportSummary>? Users { get; set; }
    }

    /// <summary>
    /// DTO para resumen de usuario en reporte
    /// </summary>
    public class UserReportSummary
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string? Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }

    /// <summary>
    /// DTO para solicitud de exportación de reporte
    /// </summary>
    public class ReportExportRequestDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Dictionary<string, string>? Filters { get; set; }
    }
}