using System.Data;
using TechChallenge4.Domain.Entities;
using TechChallenge4.Domain.Interfaces.Infra.Data.DAL;
using static Dapper.SqlMapper;

namespace TechChallenge4.Infra.Data.Dapper.DAL
{
    public class ContatoRepository : RepositoryBase<Contato>, IContatoRepository
    {
        private readonly IDbConnection _conexao;

        public ContatoRepository(IDbConnection conexao) : base(conexao)
        {
            _conexao = conexao;
        }

        protected override string ObterNomeTabela() => $"{nameof(Contato)}";
        protected override string ObterColunas() => $"{nameof(Contato.Id)},{nameof(Contato.Nome)},{nameof(Contato.Telefone)},{nameof(Contato.Email)}";
        protected override string ObterColunasInsercao() => $"{nameof(Contato.Nome)},{nameof(Contato.Telefone)},{nameof(Contato.Email)}";
        protected override string ObterValoresInsercao(Contato entidade) => $"@{nameof(Contato.Nome)},@{nameof(Contato.Telefone)},@{nameof(Contato.Email)}";
        protected override string ObterValoresAtualizacao(Contato entidade) => $"{nameof(Contato.Nome)}=@{nameof(Contato.Nome)},{nameof(Contato.Telefone)}=@{nameof(Contato.Telefone)},{nameof(Contato.Email)}=@{nameof(Contato.Email)}";

        public IEnumerable<Contato> ObterPorDdd(string ddd)
        {
            return _conexao.Query<Contato>($"SELECT {ObterColunas()} FROM {ObterNomeTabela()} WHERE SUBSTRING(Telefone,1,2)={ddd}");
        }
    }
}
