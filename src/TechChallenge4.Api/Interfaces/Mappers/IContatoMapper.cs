using TechChallenge4.Api.Dtos;
using TechChallenge4.Application.Contatos.Dto;

namespace TechChallenge4.Api.Interfaces.Mappers
{
    public interface IContatoMapper
    {
        IEnumerable<ContatoAppDto> Map(IEnumerable<ContatoDto> contatos);
        ContatoAppDto Map (ContatoDto contato);
    }
}
