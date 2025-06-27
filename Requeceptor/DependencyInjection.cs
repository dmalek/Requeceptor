using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Requeceptor.Services.Parsers;
using Requeceptor.Services.Persistence;
using Requeceptor.Services.Responses;

namespace Requeceptor;

public static class DependencyInjection
{
    public static IServiceCollection AddRequeceptor(this IServiceCollection services, Action<RequeceptorOptions> config)
    {
        services.Configure<RequeceptorOptions>(config);
        services.AddSingleton<IRequestParser, JsonRequestParser>();
        services.AddSingleton<IRequestParser, XmlRequestParser>();
        services.AddScoped<IResponseFactory, ResponseFactory>();

        return services;
    }

    public static IServiceCollection UseRequeceptorPersistence(this IServiceCollection services, Action<DatabaseOptions> config)
    {
        var options = new DatabaseOptions();
        config(options);

        var provider = options.Provider;
        var connectionId = options.ConnectionId ?? "DefaultConnection";
        var connectionString = options.ConnectionStrings[connectionId];

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

        return services;
    }

    public static WebApplication MapRequeceptorController(this WebApplication app)
    {
        RequeceptorOptions? routeOptions;
        using (var scope = app.Services.CreateScope())
        {
            routeOptions = scope.ServiceProvider.GetRequiredService<IOptions<RequeceptorOptions>>().Value;
        }        
        var apiRoute = routeOptions?.ApiRoute ?? "api";

        apiRoute = apiRoute.TrimEnd('/');

        app.MapControllerRoute(
            name: "Receptor",
            pattern: $"{apiRoute}/{{*path}}",
            defaults: new { controller = "Receptor", action = "CatchAll" });

        return app;
    }

    public static WebApplication InitializeRequeceptorPersistence(this WebApplication app)
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
