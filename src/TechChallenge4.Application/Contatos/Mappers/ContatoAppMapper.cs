using TechChallenge4.Application.Contatos.Dto;
using TechChallenge4.Application.Contatos.Interfaces;
using TechChallenge4.Domain.Entities;

namespace TechChallenge4.Application.Contatos.Mappers
{
    public class ContatoAppMapper : IContatoAppMapper
    {
        public Contato Map(ContatoAppDto contatoAppDto)
        {
            if (contatoAppDto == null) return null;
            return new Contato(contatoAppDto.Nome, contatoAppDto.Telefone, contatoAppDto.Email);
        }

        public IEnumerable<ContatoAppDto> Map(IEnumerable<Contato> contatos)
        {
            var listaRetorno = new List<ContatoAppDto>();

            foreach (var contato in contatos)
            {
                listaRetorno.Add(Map(contato));
            }

            return listaRetorno;
        }

        public ContatoAppDto Map(Contato contato)
        {
            if (contato == null) return null;

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
