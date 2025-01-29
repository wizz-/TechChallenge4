using TechChallenge4.Application.Contatos.Dto;
using TechChallenge4.Domain.Entities;

namespace TechChallenge4.Application.Contatos.Interfaces
{
    public interface IContatoAppMapper
    {
        Contato Map(ContatoAppDto contatoAppDto);
        IEnumerable<ContatoAppDto> Map(IEnumerable<Contato> enumerable);
        ContatoAppDto Map(Contato contato);
    }
}
