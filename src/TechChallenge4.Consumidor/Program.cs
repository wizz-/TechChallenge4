using Prometheus;
using TechChallenge4.Consumidor.IoC;
using TechChallenge4.Consumidor.Services.RabbitMq.Interfaces;
using TechChallenge4.Infra.DatabaseInitializers.Interfaces;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.ConfigurarEMapearDependenciasDaAplicacao();

        var app = builder.Build();

        //if (app.Environment.IsDevelopment())
        //{
            InicializarDatabase(app);
        //}

        using var scope = app.Services.CreateScope();
        var consumidorDoRabbitMq = scope.ServiceProvider.GetRequiredService<IConsumidorDoRabbitMq>();
        consumidorDoRabbitMq.ConfigurarConsumidores();

        app.MapMetrics();
        app.Run();
    }

    private static void InicializarDatabase(WebApplication app)
    {
        using var scopo = app.Services.CreateScope();
        var dbInitializer = scopo.ServiceProvider.GetService<IDatabaseInitializer>();
        if (dbInitializer == null) return;
        dbInitializer.InicializarDatabase();
    }
}
