using TechChallenge4.Api.Dtos;
using TechChallenge4.Api.Interfaces.Mappers;
using TechChallenge4.Application.Contatos.Dto;

namespace TechChallenge4.Api.Mappers
{
    public class ContatoMapper : IContatoMapper
    {
        public IEnumerable<ContatoAppDto> Map(IEnumerable<ContatoDto> contatos)
        {
            List<ContatoAppDto> listaRetorno = new List<ContatoAppDto>();

            foreach (var contato in contatos)
            {
                listaRetorno.Add(Map(contato));
            }

            return listaRetorno;
        }

        public ContatoAppDto Map(ContatoDto contato)
        {
            return new ContatoAppDto()
            {
                Id = contato.Id,
                Nome = contato.Nome,
                Telefone = contato.Telefone,
                Email = contato.Email
            };
        }
    }
}
