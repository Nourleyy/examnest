using ExamNest.Extensions;
using ExamNest.Filters;
using ExamNest.Interfaces;
using ExamNest.Middlewares;
using ExamNest.Models;
using ExamNest.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExamNest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddValidation();

            builder.Services.AddControllers();


            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddDbContext<AppDBContext>(
            options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            // TODO: Extension to builder to have the all repositories registered in single file
            builder.Services.AddScoped<IBranchRepository, BranchRepository>();
            builder.Services.AddScoped<IChoiceRepository, ChoiceRepository>();
            builder.Services.AddScoped<ICoursesRepository, CoursesRepository>();
            builder.Services.AddScoped<ITrackRepository, TracksRepository>();
            builder.Services.AddScoped<IInstructorRepository, InstructorRepository>();
            builder.Services.AddScoped<IExamRepository, ExamRepository>();
            builder.Services.AddScoped<ISubmissionRepository, SubmissionRepository>();
            builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
            builder.Services.Configure<ApiBehaviorOptions>(options =>
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

            var app = builder.Build();
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "v1"));
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
