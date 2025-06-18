using Requeceptor.Services.Parsers;
using Requeceptor.Services.RequestLoggers;

namespace Requeceptor;

public static class DependencyInjection
{
    public static IServiceCollection AddRequeceptor(this IServiceCollection services)
    {

        services.AddSingleton<IRequestParser, JsonRequestParser>();
        services.AddSingleton<IRequestParser, XmlRequestParser>();

        services.AddScoped<IRequestLoggerService, DatabaseRequestLoggerService>();

        return services;
    }
}
