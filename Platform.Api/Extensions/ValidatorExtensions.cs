using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Platform.Application.Core.App.Validators;
using Platform.Application.Core.Auth.Validators;
using Platform.Domain.DTOs.App.Reports;
using Platform.Domain.DTOs.Auth;

namespace Platform.Api.Extensions
{
    public static class ValidatorExtensions
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            // Auth validators
            services.AddScoped<IValidator<CreateUserDto>, CreateUserValidator>();
            services.AddScoped<IValidator<UpdateUserDto>, UpdateUserValidator>();
            
            // App validators
            services.AddScoped<IValidator<UsersByTypeReportRequestDto>, UsersByTypeReportRequestValidator>();
            
            return services;
        }
    }
}