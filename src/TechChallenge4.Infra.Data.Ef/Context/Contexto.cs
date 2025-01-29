using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace TechChallenge4.Infra.Data.Ef.Context
{
    public class Contexto(DbContextOptions<Contexto> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            AddConfigurations(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
        private void AddConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
