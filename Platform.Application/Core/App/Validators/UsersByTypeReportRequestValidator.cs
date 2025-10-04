using FluentValidation;
using Platform.Domain.DTOs.App.Reports;

namespace Platform.Application.Core.App.Validators
{
    public class UsersByTypeReportRequestValidator : AbstractValidator<UsersByTypeReportRequestDto>
    {
        public UsersByTypeReportRequestValidator()
        {
            RuleFor(x => x.EndDate)
                .GreaterThanOrEqualTo(x => x.StartDate)
                .When(x => x.StartDate.HasValue && x.EndDate.HasValue)
                .WithMessage("La fecha final debe ser mayor o igual a la fecha inicial");
        }
    }
}