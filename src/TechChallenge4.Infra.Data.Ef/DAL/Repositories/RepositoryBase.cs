using DelegateDecompiler;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TechChallenge4.Domain.Interfaces.Infra.Data.DAL;
using TechChallenge4.Infra.Data.Ef.Context;

namespace TechChallenge4.Infra.Data.Ef.DAL.Repositories
{
    public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        protected Contexto Context;
        protected DbSet<TEntity> DbSet;

        protected RepositoryBase(Contexto context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }

        public void Atualizar(TEntity entidade)
        {
            DbSet.Attach(entidade);
            Context.Entry(entidade).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public void Excluir(int id)
        {
            var entidade = DbSet.Find(id);

            if (Context.Entry(entidade).State == EntityState.Detached)
            {
                DbSet.Attach(entidade);
            }

            DbSet.Remove(entidade);
            Context.SaveChanges();
        }

        public int Inserir(TEntity entidade)
        {
            DbSet.Add(entidade);
            Context.SaveChanges();

            var myType = entidade.GetType();
            var props = new List<PropertyInfo>(myType.GetProperties());

            foreach (PropertyInfo prop in props)
            {
                if (prop.Name == "Id") return (int)prop.GetValue(entidade, null);
            }

            throw new InvalidOperationException("Não foi possível recuprar o Id após a inclusão.");
        }

        public TEntity ObterPorId(int id)
        {
            return DbSet.Find(id);
        }

        public IEnumerable<TEntity> ObterTodos()
        {
            return ObterQueryable();
        }

        internal IQueryable<TEntity> ObterQueryable()
        {
            IQueryable<TEntity> query = DbSet;

            return query.Decompile();
        }
    }
}
