using TechChallenge4.Domain.Entities;
using TechChallenge4.Domain.Interfaces.Infra.Data.DAL;
using TechChallenge4.Infra.Data.Ef.Context;

namespace TechChallenge4.Infra.Data.Ef.DAL.Repositories
{
    public class ContatoRepository(Contexto context) : RepositoryBase<Contato>(context), IContatoRepository
    {
        public IEnumerable<Contato> ObterPorDdd(string ddd)
        {
            return ObterQueryable().Where(x => x.Telefone.Substring(0, 2) == ddd);
        }
    }
}
