using ExamNest.Extensions;
using ExamNest.Filters;
using ExamNest.Middlewares;
using ExamNest.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamNest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers(options => options.Filters.Add<ApiResponseFilter>());


            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddDbContext<AppDBContext>(
            options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddRepositories();
            builder.Services.AddValidation();



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
