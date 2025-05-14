using ExamNest.Filters;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace ExamNest.Extensions
{
    public static class Validations
    {
        public static IServiceCollection AddValidation(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<Program>();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                                        .Where(x => x.Value?.Errors.Count > 0)
                                        .SelectMany(x => x.Value!.Errors)
                                        .Select(x => x.ErrorMessage)
                                        .ToList();

                    var errorResponse = new ApiResponse<object>()
                    {
                        Message = "Validation Failed",
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });
            return services;
        }
    }
}
