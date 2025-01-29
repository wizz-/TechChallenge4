using TechChallenge4.Api.Dtos;

namespace TechChallenge4.Api.Interfaces.Services
{
    public interface IMensageriaDeContatosService
    {
        Task EnviarContatoParaGravacao(string fila, ContatoDto contato);
        Task EnviarMensagemDeExclusao(string fila, int id);
    }
}
