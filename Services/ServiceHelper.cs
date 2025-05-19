namespace ExamNest.Services
{
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceHelper
    {
        public static IServiceProvider ServiceProvider { get; set; }

        public static T GetRequiredService<T>() where T : class
        {
            return ServiceProvider.GetRequiredService<T>();
        }
    }

}
