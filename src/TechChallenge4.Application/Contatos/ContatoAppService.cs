using TechChallenge4.Application.Contatos.Dto;
using TechChallenge4.Application.Contatos.Interfaces;
using TechChallenge4.Domain.Interfaces.Infra.Data.DAL;

namespace TechChallenge4.Application.Contatos
{
    public class ContatoAppService(IContatoRepository contatoRepository, IContatoAppMapper _mapper) : IContatoAppService
    {
        public IEnumerable<ContatoAppDto> ObterTodos()
        {
            return _mapper.Map(contatoRepository.ObterTodos());
        }

        public int Inserir(ContatoAppDto contatoAppDto)
        {
            var contato = _mapper.Map(contatoAppDto);
            return contatoRepository.Inserir(contato);
        }

        public ContatoAppDto ObterPorId(int id)
        {
            var contato = contatoRepository.ObterPorId(id);
            return _mapper.Map(contato);
        }

        public void Atualizar(ContatoAppDto contatoAppDto)
        {
            var contato = contatoRepository.ObterPorId(contatoAppDto.Id);
            if (contato == null) throw new KeyNotFoundException($"Não foi localizado contato com id {contatoAppDto.Id}");

            contato.AtualizarDados(contatoAppDto.Nome, contatoAppDto.Telefone, contatoAppDto.Email);
            contatoRepository.Atualizar(contato);
        }

        public void Excluir(int id)
        {
            var contato = contatoRepository.ObterPorId(id);
            if (contato == null) throw new KeyNotFoundException($"Não foi localizado contato com id {id}");
            contatoRepository.Excluir(id);
        }

        public IEnumerable<ContatoAppDto> ObterPorDdd(string ddd)
        {
            return _mapper.Map(contatoRepository.ObterPorDdd(ddd));
        }
    }
}
