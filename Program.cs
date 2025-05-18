using ExamNest.Extensions;
using ExamNest.Filters;
using ExamNest.Middlewares;
using ExamNest.Models;
using ExamNest.Services;
using Microsoft.EntityFrameworkCore;

namespace ExamNest
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers(options => options.Filters.Add<ApiResponseFilter>());


            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddDbContext<AppDBContext>(
            options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddRepositories();
            builder.Services.AddIdentity(builder.Configuration);

            builder.Services.AddValidation();

            builder.Services.AddScoped<UserManagementService>();
            builder.Services.AddScoped<TokenManagementService>();



            var app = builder.Build();
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "v1"));
            }
            app.Use(async (context, next) =>
            {
                var token = context.Request.Cookies["ExamNest.Token"];
                if (!string.IsNullOrEmpty(token))
                {
                    context.Request.Headers.Authorization = "Bearer " + token;
                }
                await next();
            });

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            // Apply migrations and seed data.
            using (var scope = app.Services.CreateScope())
            {
                await DataSeeder.InitializeAsync(scope.ServiceProvider);
            }

            app.MapControllers();

            app.Run();
        }
    }
}
