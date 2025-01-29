using RabbitMQ.Client;
using TechChallenge4.Consumidor.Mappers;
using TechChallenge4.Consumidor.Mappers.Interfaces;
using TechChallenge4.Consumidor.Services.RabbitMq;
using TechChallenge4.Consumidor.Services.RabbitMq.Interfaces;
using TechChallenge4.Infra.IoC;
using TechChallenge4.Infra.IoC.Enums;

namespace TechChallenge4.Consumidor.IoC
{
    public static class MapeamentosDaApi
    {
        public static void ConfigurarEMapearDependenciasDaAplicacao(this WebApplicationBuilder builder)
        {
            ConfigurarRabbitMqAsync(builder.Services).GetAwaiter().GetResult();
            MapearApi(builder.Services);
            MapeamentosDaAplicacao.Mapear(builder.Services, TiposDeOrm.EntityFramework);
        }

        private static async Task ConfigurarRabbitMqAsync(IServiceCollection services)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "FilaParaInserir", durable: false, exclusive: false, autoDelete: false, arguments: null);
            await channel.QueueDeclareAsync(queue: "FilaParaAlterar", durable: false, exclusive: false, autoDelete: false, arguments: null);
            await channel.QueueDeclareAsync(queue: "FilaParaExcluir", durable: false, exclusive: false, autoDelete: false, arguments: null);

            services.AddSingleton(channel);
        }

        private static void MapearApi(IServiceCollection services)
        {
            services.AddScoped<IContatoMapper, ContatoMapper>();
            services.AddScoped<IConsumidorDoRabbitMq, ConsumidorDoRabbitMq>();
        }
    }
}
