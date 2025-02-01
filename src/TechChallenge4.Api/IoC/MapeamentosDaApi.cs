using Carter;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
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

            Task.Run(async() => { await ConfigurarRabbitMq(builder.Services, builder.Configuration); }).Wait();

            MapearApi(builder.Services);
            MapeamentosDaAplicacao.Mapear(builder.Services, tipoDeOrm);
        }

        private async static Task ConfigurarRabbitMq(IServiceCollection services, IConfiguration configuration)
        {
            var factory = new ConnectionFactory
            {
                HostName = configuration["RabbitMq:HostName"],
                UserName = configuration["RabbitMq:UserName"],
                Password = configuration["RabbitMq:Password"],
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };

            var retryPolicy = Policy
                .Handle<BrokerUnreachableException>()
                .Or<ConnectFailureException>()
                .WaitAndRetryAsync(20, retryAttempt => TimeSpan.FromSeconds(5),
                    (exception, timeSpan, retryCount, context) =>
                    {
                        Console.WriteLine($"RabbitMQ: Tentativa {retryCount} falhou: {exception.Message}. Tentando novamente em {timeSpan.Seconds} segundos.");
                    });

            try
            {
                await retryPolicy.ExecuteAsync(async () =>
                {
                    var connection = await factory.CreateConnectionAsync();
                    var channel = await connection.CreateChannelAsync();

                    await channel.QueueDeclareAsync(queue: "FilaParaInserir", durable: false, exclusive: false, autoDelete: false, arguments: null);
                    await channel.QueueDeclareAsync(queue: "FilaParaAlterar", durable: false, exclusive: false, autoDelete: false, arguments: null);
                    await channel.QueueDeclareAsync(queue: "FilaParaExcluir", durable: false, exclusive: false, autoDelete: false, arguments: null);

                    services.AddSingleton(channel);
                    Console.WriteLine("Conectado com sucesso ao RabbitMQ e filas declaradas.");
                });
            }
            catch (BrokerUnreachableException ex)
            {
                Console.WriteLine($"Erro ao conectar ao RabbitMQ após múltiplas tentativas: {ex.Message}");
            }
        }

        private static void MapearApi(IServiceCollection services)
        {
            services.AddSingleton<IContatoMapper, ContatoMapper>();
            services.AddSingleton<IMensageriaDeContatosService, MensageriaDeContatosService>();
        }
    }
}
