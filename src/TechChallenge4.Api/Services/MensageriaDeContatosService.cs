using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using System.Threading.Channels;
using TechChallenge4.Api.Dtos;
using TechChallenge4.Api.Interfaces.Services;
using TechChallenge4.Domain.Entities;

namespace TechChallenge4.Api.Services
{
    public class MensageriaDeContatosService(IChannel channel) : IMensageriaDeContatosService
    {
        public async Task EnviarContatoParaGravacao(string fila, ContatoDto contato)
        {
            var message = JsonSerializer.Serialize(contato); 
            var body = Encoding.UTF8.GetBytes(message);

            var properties = new BasicProperties
            {
                Persistent = true
            };

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: fila,
                mandatory: false,
                basicProperties: properties,
                body: body
            );
        }

        public async Task EnviarMensagemDeExclusao(string fila, int id)
        {
            var message = JsonSerializer.Serialize(id);
            var body = Encoding.UTF8.GetBytes(message);

            var properties = new BasicProperties
            {
                Persistent = true
            };

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: fila,
                mandatory: false,
                basicProperties: properties,
                body: body
            );
        }
    }
}
