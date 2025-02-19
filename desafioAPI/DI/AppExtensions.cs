using desafioAPI.Bus;
using MassTransit;

namespace desafioAPI.DI
{
    internal static class AppExtensions
    {

        public static void AddRabbitMQService(this IServiceCollection services)
        {

            services.AddMassTransit(busConfigurator =>
            {
                //busConfigurator.AddConsumer<TransactionCreatedEventConsumer>();

                busConfigurator.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(new Uri("amqp://localhost"), host =>
                    {
                        host.Username("guest");
                        host.Password("guest");
                    });

                    //cfg.ConfigureEndpoints(ctx);

                });

                
            });


        }

    }
}
