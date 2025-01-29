using TechChallenge4.Infra.Data.Ef.Context;
using TechChallenge4.Infra.DatabaseInitializers.Interfaces;

namespace TechChallenge4.Infra.DatabaseInitializers
{
    public class DatabaseInitializer(Contexto context) : IDatabaseInitializer
    {
        public void InicializarDatabase()
        {
            context.Database.EnsureCreated();
        }
    }
}
