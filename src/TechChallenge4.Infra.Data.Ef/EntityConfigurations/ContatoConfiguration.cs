using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechChallenge4.Domain.Entities;

namespace TechChallenge4.Infra.Data.Ef.EntityConfigurations
{
    public class ContatoConfiguration : IEntityTypeConfiguration<Contato>
    {
        public void Configure(EntityTypeBuilder<Contato> builder)
        {
            builder.Property(x => x.Telefone)
                .HasMaxLength(11);

            builder.Property(x => x.Email)
                .HasMaxLength(150);

            builder.Property(x => x.Nome)
                .HasMaxLength(100);
        }
    }
}
