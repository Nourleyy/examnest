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
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                                  corsPolicyBuilder => corsPolicyBuilder.AllowAnyOrigin()
                                                                        .AllowAnyMethod()
                                                                        .AllowAnyHeader());
            });

            builder.Services.AddControllers(options => options.Filters.Add<ApiResponseFilter>());

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddDbContext<AppDBContext>(options =>
                                                            options.UseSqlServer(builder.Configuration
                                                                .GetConnectionString("DefaultConnection")));

            builder.Services.AddRepositories();
            builder.Services.AddServices();

            builder.Services.AddIdentity(builder.Configuration);

            builder.Services.AddValidation();


            builder.Services.AddSwagger();

            var app = builder.Build();
            app.UseCors("CorsPolicy");

            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
            app.UseMiddleware<TokenInjectionMiddleware>();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "ExamNest API V1"); });

            ServiceHelper.ServiceProvider = app.Services;

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            // await app.Services.StartSeedAsync();
            // DataSeeder.InitializeAsync(app.Services, new SeedCounts());
            await app.RunAsync();
        }
    }
}