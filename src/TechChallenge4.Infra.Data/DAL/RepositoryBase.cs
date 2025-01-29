using Dapper;
using System.Data;
using TechChallenge4.Domain.Interfaces.Infra.Data.DAL;

namespace TechChallenge4.Infra.Data.Dapper.DAL
{
    public abstract class RepositoryBase<TEntity>(IDbConnection _conexao) : IRepositoryBase<TEntity> where TEntity : class
    {
        protected abstract string ObterNomeTabela();
        protected abstract string ObterColunas();
        protected abstract string ObterColunasInsercao();
        protected abstract string ObterValoresInsercao(TEntity entidade);
        protected abstract string ObterValoresAtualizacao(TEntity entidade);

        public IEnumerable<TEntity> ObterTodos()
        {
            return _conexao.Query<TEntity>($"SELECT {ObterColunas()} FROM {ObterNomeTabela()}");
        }

        public TEntity ObterPorId(int id)
        {
            return _conexao.QueryFirstOrDefault<TEntity>($"SELECT {ObterColunas()} FROM {ObterNomeTabela()} WHERE ID = {id}");
        }

        public int Inserir(TEntity entidade)
        {
            var sql = @$"INSERT INTO {ObterNomeTabela()}({ObterColunasInsercao()}) 
                      OUTPUT INSERTED.Id
                      VALUES({ObterValoresInsercao(entidade)})";

            return _conexao.QuerySingle<int>(sql, entidade);
        }

        public void Atualizar(TEntity entidade)
        {
            var sql = $"UPDATE {ObterNomeTabela()} SET {ObterValoresAtualizacao(entidade)} WHERE Id = @Id";
            _conexao.Execute(sql, entidade);
        }

        public void Excluir(int id)
        {
            _conexao.Execute($"DELETE FROM {ObterNomeTabela()} WHERE Id = @Id", new { Id = id });
        }
    }
}
