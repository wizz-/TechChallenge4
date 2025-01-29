using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using TechChallenge4.Application.Contatos;
using TechChallenge4.Application.Contatos.Interfaces;
using TechChallenge4.Application.Contatos.Mappers;
using TechChallenge4.Domain.Interfaces.Infra.Data.DAL;
using TechChallenge4.Infra.Data.Dapper.Contexto;
using TechChallenge4.Infra.Data.Ef.Context;
using TechChallenge4.Infra.DatabaseInitializers;
using TechChallenge4.Infra.DatabaseInitializers.Interfaces;
using TechChallenge4.Infra.IoC.Enums;
using DapperDal = TechChallenge4.Infra.Data.Dapper.DAL;
using EfDal = TechChallenge4.Infra.Data.Ef.DAL.Repositories;

namespace TechChallenge4.Infra.IoC
{
    public static class MapeamentosDaAplicacao
    {
        public static void Mapear(IServiceCollection services, TiposDeOrm tipoDeOrm)
        {
            MapearApplication(services);
            MapearInfra(services, tipoDeOrm);
        }

        private static void MapearApplication(IServiceCollection services)
        {
            services.AddScoped<IContatoAppService, ContatoAppService>();
            services.AddScoped<IContatoAppMapper, ContatoAppMapper>();
        }

        private static void MapearInfra(IServiceCollection services, TiposDeOrm tipoDeOrm)
        {
            if (tipoDeOrm == TiposDeOrm.Dapper)
            {
                MapearDapper(services);
                return;
            }

            if(tipoDeOrm == TiposDeOrm.EntityFramework)
            {
                MapearEf(services);
                return;
            }

            if(tipoDeOrm == TiposDeOrm.EntityFrameworkInMemorian)
            {
                MapearEfInMemorian(services);
            }
        }

        private static void MapearDapper(IServiceCollection services)
        {
            services.AddSingleton<IDbConnection>(
                                provider => new DbConnectionFactory(provider.GetRequiredService<IConfiguration>()).GetConnection()
                            );
            services.AddScoped<IContatoRepository, DapperDal.ContatoRepository>();
        }

        private static void MapearEf(IServiceCollection services)
        {
            services.AddDbContext<Contexto>((serviceProvider, options) =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IContatoRepository, EfDal.ContatoRepository>();
            services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();
        }

        private static void MapearEfInMemorian(IServiceCollection services)
        {
            services.AddScoped<Contexto>((serviceProvider) =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();

                var builder = new DbContextOptionsBuilder<Contexto>()
                                    .UseInMemoryDatabase("TestDb");

                return new Contexto(builder.Options);
            });

            services.AddScoped<IContatoRepository, EfDal.ContatoRepository>();
            services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();
        }
    }
}
