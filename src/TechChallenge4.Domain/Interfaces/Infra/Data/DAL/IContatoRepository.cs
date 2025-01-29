using TechChallenge4.Domain.Entities;

namespace TechChallenge4.Domain.Interfaces.Infra.Data.DAL
{
    public interface IContatoRepository : IRepositoryBase<Contato>
    {
        IEnumerable<Contato> ObterPorDdd(string ddd);
    }
}
