using ExamNest.Interfaces;
using ExamNest.Repositories;

namespace ExamNest.Extensions
{
    public static class Repositories
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped<IChoiceRepository, ChoiceRepository>();
            services.AddScoped<ICoursesRepository, CoursesRepository>();
            services.AddScoped<ITrackRepository, TracksRepository>();
            services.AddScoped<IInstructorRepository, InstructorRepository>();
            services.AddScoped<IExamRepository, ExamRepository>();
            services.AddScoped<ISubmissionRepository, SubmissionRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            return services;
        }
    }
}
