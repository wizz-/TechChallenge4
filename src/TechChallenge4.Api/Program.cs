using Carter;
using Prometheus;
using TechChallenge4.Api.IoC;
using TechChallenge4.Infra.DatabaseInitializers.Interfaces;
using TechChallenge4.Infra.IoC.Enums;
using TechChallenge4.Api.Middlewares;

namespace TechChallenge4.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var tipoDeOrm = TiposDeOrm.EntityFramework;
            var builder = WebApplication.CreateBuilder(args);
            builder.ConfigurarEMapearDependenciasDaAplicacao(tipoDeOrm);

            var app = builder.Build();

            //if (app.Environment.IsDevelopment())
            //{
                InicializarDatabase(app, tipoDeOrm);

                app.UseSwagger();
                app.UseSwaggerUI();
            //}

            app.UseMiddleware<MetricsMiddleware>();
            app.UseAuthorization();
            app.UseStatusCodePages();
            app.MapCarter();

            app.MapMetrics();

            app.Run();
        }

        private static void InicializarDatabase(WebApplication app, TiposDeOrm tipoDeOrm)
        {
            if (tipoDeOrm == TiposDeOrm.EntityFramework)
            {
                using var scopo = app.Services.CreateScope();
                var dbInitializer = scopo.ServiceProvider.GetService<IDatabaseInitializer>();
                if (dbInitializer == null) return;
                dbInitializer.InicializarDatabase();
            }
        }
    }
}
