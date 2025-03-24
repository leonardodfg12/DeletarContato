using System.Diagnostics.CodeAnalysis;
using DeletarContato.Infrastructure.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeletarContato.Infrastructure.Config
{
    [ExcludeFromCodeCoverage]
    public static class MassTransitConfig
    {
        public static void AddMassTransitConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitMqHost = configuration["RABBITMQ_HOST"];

            services.AddMassTransit(x =>
            {
                x.AddConsumer<ContatoConsumer>();
                
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMqHost);
                    
                    cfg.ReceiveEndpoint("deletar-contato-queue", e =>
                    {
                        e.ConfigureConsumer<ContatoConsumer>(context);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });
        }
    }
}