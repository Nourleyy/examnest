using Microsoft.OpenApi.Models;

namespace ExamNest.Extensions
{
    public static class Swagger
    {
        private static OpenApiInfo ApiInfo => new() { Title = "ExamNest API", Version = "v1" };

        private static OpenApiSecurityScheme ApiSecurityScheme => new()
        {
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer"
        };
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", ApiInfo);

                options.AddSecurityDefinition("Bearer", ApiSecurityScheme);

                options.AddSecurityRequirement(SecurityRequirement);
            });

            return services;

        }

        private static OpenApiSecurityRequirement SecurityRequirement =>
            new()
            {

                {
                    Key,
                    []
                }
            };

        private static OpenApiSecurityScheme Key =>
            new()
            {
                Reference = Reference
            };

        private static OpenApiReference Reference =>
            new()
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            };
    }
}
