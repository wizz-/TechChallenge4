namespace TechChallenge4.Domain.Interfaces.Infra.Data.DAL
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> ObterTodos();
        TEntity ObterPorId(int id);
        int Inserir(TEntity entidade);
        void Atualizar(TEntity entidade);
        void Excluir(int id);
    }
}
