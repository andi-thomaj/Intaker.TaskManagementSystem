using Intaker.TaskManagementSystem.infrastructure.EntityFramework;
using Intaker.TaskManagementSystem.infrastructure.EntityFramework.TaskManagement.Repository;
using MassTransit;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

namespace Intaker.TaskManagementSystem.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;
            var services = builder.Services;

            services.AddControllers()
                .AddMvcOptions(options =>
                {
                    options.Conventions.Add(new RouteTokenTransformerConvention(new LowercaseRouteTransformer()));
                });
            services.AddEndpointsApiExplorer();
            services.AddOpenApi();
            services.AddMassTransit(config =>
            {
                config.UsingAzureServiceBus((ctx, cfg) =>
                {
                    cfg.Host(configuration.GetConnectionString("AzureServiceBus"));
                });
            });
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
            services.AddFluentValidation([typeof(Program).Assembly]);
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<ITaskRepository, TaskRepository>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference(options =>
                {
                    options.Title = "intaker-taskManagement-api";
                    options.Theme = ScalarTheme.Kepler;
                    options.HideDownloadButton = true;
                });
            }

            app.UseCustomExceptionMiddleware();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }

        private class LowercaseRouteTransformer : IOutboundParameterTransformer
        {
            public string? TransformOutbound(object? value)
            {
                return value?.ToString()?.ToLowerInvariant();
            }
        }
    }
}
