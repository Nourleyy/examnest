using ExamNest.Models;

namespace ExamNest.Extensions
{
    public static class Seeder
    {
        public static async Task StartSeedAsync(this IServiceProvider services)
        {
            using var scope = services.CreateScope();
            await DataSeeder.InitializeAsync(scope.ServiceProvider);
        }
    }
}
