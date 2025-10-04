using Platform.Domain.DTOs.App.Reports;
using System.Threading;
using System.Threading.Tasks;

namespace Platform.Application.Core.App.Queries.Reports
{
    public interface IReportQueryHandler
    {
        Task<UsersByTypeReportDto> GetUsersByTypeReport(UsersByTypeReportRequestDto requestDto, CancellationToken cancellationToken);
        Task<byte[]> ExportUsersByTypeReport(UsersByTypeReportRequestDto requestDto, CancellationToken cancellationToken);
    }
}