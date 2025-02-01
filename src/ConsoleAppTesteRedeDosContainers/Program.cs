using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;


namespace ConsoleAppTesteRedeDosContainers
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var factory = new ConnectionFactory
            {
                HostName = "rabbitmq",
                UserName = "guest",
                Password = "guest",
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };

            var retryPolicy = Policy
                .Handle<BrokerUnreachableException>()
                .Or<ConnectFailureException>()
                .WaitAndRetryAsync(10, retryAttempt => TimeSpan.FromSeconds(10),
                    (exception, timeSpan, retryCount, context) =>
                    {
                        Console.WriteLine($"Tentativa {retryCount} falhou: {exception.Message}. Tentando novamente em {timeSpan.Seconds} segundos.");
                    });

            try
            {
                await retryPolicy.ExecuteAsync(async () =>
                {
                    using var connection = await factory.CreateConnectionAsync();
                    using var channel = await connection.CreateChannelAsync();

                    Console.WriteLine("Conectado com sucesso ao RabbitMQ!");

                    await channel.QueueDeclareAsync(queue: "FilaParaInserir", durable: false, exclusive: false, autoDelete: false, arguments: null);
                    Console.WriteLine("Fila 'FilaParaInserir' declarada com sucesso!");
                });
            }
            catch (BrokerUnreachableException ex)
            {
                Console.WriteLine($"Erro ao conectar ao RabbitMQ após múltiplas tentativas: {ex.Message}");
            }
        }
    }
}
