using TechChallenge4.Application.Contatos.Dto;

namespace TechChallenge4.Application.Contatos.Interfaces
{
    public interface IContatoAppService
    {
        ContatoAppDto ObterPorId(int id);
        IEnumerable<ContatoAppDto> ObterTodos();
        IEnumerable<ContatoAppDto> ObterPorDdd(string ddd);
        int Inserir(ContatoAppDto contatoAppDto);
        void Atualizar(ContatoAppDto contatoAppDto);
        void Excluir(int id);
    }
}
