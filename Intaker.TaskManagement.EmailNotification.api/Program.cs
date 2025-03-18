
using MassTransit;
using Scalar.AspNetCore;

namespace Intaker.TaskManagement.EmailNotification.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;
            var services = builder.Services;

            services.AddAuthorization();
            services.AddEndpointsApiExplorer();
            services.AddOpenApi();
            services.AddMassTransit(config =>
            {
                config.AddConsumer<TaskCreatedEmailNotificationConsumer>();
                config.AddConsumer<TaskUpdatedEmailNotificationConsumer>();

                config.UsingAzureServiceBus((ctx, cfg) =>
                {
                    cfg.Host(builder.Configuration.GetConnectionString("AzureServiceBus"));
                    cfg.EnableDuplicateDetection(TimeSpan.FromMinutes(5));
                    cfg.LockDuration = TimeSpan.FromMinutes(5);
                    cfg.ReceiveEndpoint("email-notification-task-created", e =>
                    {

                    
                        e.UseMessageRetry(r =>
                        {
                            r.Exponential(
                                retryLimit: 5,
                                minInterval: TimeSpan.FromSeconds(1),
                                maxInterval: TimeSpan.FromSeconds(30),
                                intervalDelta: TimeSpan.FromSeconds(5)
                            );
                            r.Handle<Azure.Messaging.ServiceBus.ServiceBusException>();
                        });

                        e.UseCircuitBreaker(cb =>
                        {
                            cb.TrackingPeriod = TimeSpan.FromMinutes(1);
                            cb.TripThreshold = 15;
                            cb.ActiveThreshold = 10;
                            cb.ResetInterval = TimeSpan.FromMinutes(5);
                        });

                        e.ConfigureConsumer<TaskCreatedEmailNotificationConsumer>(ctx);
                    });

                    cfg.ReceiveEndpoint("email-notification-task-updated", e =>
                    {
                        e.UseMessageRetry(r =>
                        {
                            r.Exponential(
                                retryLimit: 5,
                                minInterval: TimeSpan.FromSeconds(1),
                                maxInterval: TimeSpan.FromSeconds(30),
                                intervalDelta: TimeSpan.FromSeconds(5)
                            );
                            r.Handle<Azure.Messaging.ServiceBus.ServiceBusException>();
                        });

                        e.UseCircuitBreaker(cb =>
                        {
                            cb.TrackingPeriod = TimeSpan.FromMinutes(1);
                            cb.TripThreshold = 15;
                            cb.ActiveThreshold = 10;
                            cb.ResetInterval = TimeSpan.FromMinutes(5);
                        });

                        e.ConfigureConsumer<TaskUpdatedEmailNotificationConsumer>(ctx);
                    });
                });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference(options =>
                {
                    options.Title = "intaker-taskManagement-emailNotification-api";
                    options.Theme = ScalarTheme.Kepler;
                    options.HideDownloadButton = true;
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.Run();
        }
    }
}
