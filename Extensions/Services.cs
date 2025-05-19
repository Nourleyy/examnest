using ExamNest.Interfaces;
using ExamNest.Services;

namespace ExamNest.Extensions
{
    public static class Services
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserManagement, UserManagementService>();
            services.AddScoped<ITokenManagementService, TokenManagementService>();

            return services;
        }
    }
}
