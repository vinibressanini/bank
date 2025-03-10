using desafioAPI.consumer.Bus;
using MassTransit;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace desafioAPI.consumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();

            builder.Services.AddHttpClient();
            builder.Services.AddMassTransit(busConfigurator =>
            {

                busConfigurator.AddConsumer<TransactionCreatedEventConsumer>();

                busConfigurator.AddConfigureEndpointsCallback((context, name, cfg) =>
                {
                    cfg.UseDelayedRedelivery(r => r.Intervals(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(5)));
                    //cfg.UseMessageRetry(r => r.Interval(5,TimeSpan.FromSeconds(10);
                });
                

                busConfigurator.UsingRabbitMq((ctx, cfg) =>
                {

                    cfg.Host(new Uri($"amqp://{builder.Configuration["RabbitMQ:Host"]}"), host =>
                    {

                        host.Username(builder.Configuration["RabbitMQ:Username"] ?? "guest");
                        host.Password(builder.Configuration["RabbitMQ:Password"] ?? "guest");
                    });

                    cfg.ConfigureEndpoints(ctx);

                });

            });


            var app = builder.Build();
            app.Run();
        }
    }
}