using TechChallenge4.Application.Contatos.Dto;
using TechChallenge4.Consumidor.Dtos;
using TechChallenge4.Consumidor.Mappers.Interfaces;

namespace TechChallenge4.Consumidor.Mappers
{
    public class ContatoMapper : IContatoMapper
    {
        public IEnumerable<ContatoAppDto> Map(IEnumerable<ContatoDto> contatos)
        {
            var listaRetorno = new List<ContatoAppDto>();

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
