using ExamNest.DTO;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace ExamNest.Extensions
{
    public static class Validations
    {
        public static IServiceCollection AddValidation (this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<Program>();
            return services;
        }
    }
}
