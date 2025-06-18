using Requeceptor.Domain;

namespace Requeceptor.Services.RequestLoggers;

public interface IRequestLoggerService
{
    Task SaveAsync(RequestRecord request);
}