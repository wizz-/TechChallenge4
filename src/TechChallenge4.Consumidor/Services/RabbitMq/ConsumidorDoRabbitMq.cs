using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using TechChallenge4.Application.Contatos.Interfaces;
using TechChallenge4.Consumidor.Dtos;
using TechChallenge4.Consumidor.Mappers.Interfaces;
using TechChallenge4.Consumidor.Services.RabbitMq.Interfaces;

namespace TechChallenge4.Consumidor.Services.RabbitMq
{
    public class ConsumidorDoRabbitMq(IChannel _channel, 
        IContatoAppService _contatoAppService, 
        IContatoMapper _contatoMapper) : IConsumidorDoRabbitMq
    {
        public void ConfigurarConsumidores()
        {
            ConfigurarConsumidorDeInclusao();
            ConfigurarConsumidorDeAlteracao();
            ConfigurarConsumidorDeExclusao();
        }
        private void ConfigurarConsumidorDeInclusao()
        {
            var consumidor = new AsyncEventingBasicConsumer(_channel); 
            
            consumidor.ReceivedAsync += async (model, ea) => { 
                var body = ea.Body.ToArray(); 
                var message = Encoding.UTF8.GetString(body); 
                var contato = JsonSerializer.Deserialize<ContatoDto>(message); 
                _contatoAppService.Inserir(_contatoMapper.Map(contato!)); 
            }; 
            
            _channel.BasicConsumeAsync(queue: "FilaParaInserir", autoAck: true, consumer: consumidor);
        }
        private void ConfigurarConsumidorDeAlteracao()
        {
            var consumidor = new AsyncEventingBasicConsumer(_channel);

            consumidor.ReceivedAsync += async (model, ea) => {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var contato = JsonSerializer.Deserialize<ContatoDto>(message);
                _contatoAppService.Atualizar(_contatoMapper.Map(contato!));
            };

            _channel.BasicConsumeAsync(queue: "FilaParaAlterar", autoAck: true, consumer: consumidor);
        }

        private void ConfigurarConsumidorDeExclusao()
        {
            var consumidor = new AsyncEventingBasicConsumer(_channel);

            consumidor.ReceivedAsync += async (model, ea) => {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var id = JsonSerializer.Deserialize<int>(message);
                _contatoAppService.Excluir(id);
            };

            _channel.BasicConsumeAsync(queue: "FilaParaExcluir", autoAck: true, consumer: consumidor);
        }
    }
}
