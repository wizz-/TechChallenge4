using Carter;
using RabbitMQ.Client;
using TechChallenge4.Api.Interfaces.Mappers;
using TechChallenge4.Api.Interfaces.Services;
using TechChallenge4.Api.Mappers;
using TechChallenge4.Api.Services;
using TechChallenge4.Infra.IoC;
using TechChallenge4.Infra.IoC.Enums;

namespace TechChallenge4.Api.IoC
{
    public static class MapeamentosDaApi
    {
        public static void ConfigurarEMapearDependenciasDaAplicacao(this WebApplicationBuilder builder, TiposDeOrm tipoDeOrm)
        {            
            builder.Services.AddAuthorization();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCarter();

            Task.Run(async() => { await ConfigurarRabbitMq(builder.Services); }).Wait();

            MapearApi(builder.Services);
            MapeamentosDaAplicacao.Mapear(builder.Services, tipoDeOrm);
        }

        private async static Task ConfigurarRabbitMq(IServiceCollection services)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };
            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "FilaParaInserir", durable: false, exclusive: false, autoDelete: false, arguments: null);
            await channel.QueueDeclareAsync(queue: "FilaParaAlterar", durable: false, exclusive: false, autoDelete: false, arguments: null);
            await channel.QueueDeclareAsync(queue: "FilaParaExcluir", durable: false, exclusive: false, autoDelete: false, arguments: null);

            services.AddSingleton(channel);
        }

        private static void MapearApi(IServiceCollection services)
        {
            services.AddSingleton<IContatoMapper, ContatoMapper>();
            services.AddSingleton<IMensageriaDeContatosService, MensageriaDeContatosService>();
        }
    }
}
