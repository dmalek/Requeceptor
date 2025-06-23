using Microsoft.EntityFrameworkCore;
using Requeceptor.Services.Parsers;
using Requeceptor.Services.Persistence;
using Requeceptor.Services.Responses;

namespace Requeceptor;

public static class DependencyInjection
{
    public static IServiceCollection AddRequeceptor(this IServiceCollection services, IConfiguration configuration)
    {
        var provider = configuration["DatabaseProvider"];
        var connectionId = configuration["ConnectionId"];
        var connectionString = configuration.GetConnectionString(connectionId);

        services.AddDbContext<DatabaseContext>(options =>
        {
            switch (provider?.ToLowerInvariant())
            {
                case "sqlserver":
                    options.UseSqlServer(connectionString);
                    break;
                case "sqlite":
                    options.UseSqlite(connectionString);
                    break;
                case "inmemory":
                    options.UseInMemoryDatabase("Requeceptor.db");
                    break;
                default:
                    throw new InvalidOperationException($"Unsupported provider: {provider}");
            }
        });

        services.AddScoped<IPersistenceService, DatabaseService>();
        services.AddSingleton<IRequestParser, JsonRequestParser>();
        services.AddSingleton<IRequestParser, XmlRequestParser>();
        services.AddScoped<IResponseFactory, ResponseFactory>();
        

        return services;
    }

    public static WebApplication MapRequeceptorRoute(this WebApplication app)
    {
        app.MapControllerRoute(
            name: "Receptor",
            pattern: "r/{*path}",
            defaults: new { controller = "Receptor", action = "CatchAll" });

        return app;
    }

    public static WebApplication InitializePersistence(this WebApplication app)
    {
        // OVDJE pozivaš EnsureCreated
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            db.Database.EnsureCreated();
        }

        return app;
    }
}
