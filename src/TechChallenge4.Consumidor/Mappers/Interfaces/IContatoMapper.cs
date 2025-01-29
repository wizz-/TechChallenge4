using TechChallenge4.Application.Contatos.Dto;
using TechChallenge4.Consumidor.Dtos;

namespace TechChallenge4.Consumidor.Mappers.Interfaces
{
    public interface IContatoMapper
    {
        IEnumerable<ContatoAppDto> Map(IEnumerable<ContatoDto> contatos);
        ContatoAppDto Map(ContatoDto contato);
    }
}
