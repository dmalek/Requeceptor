using Requeceptor.Domain;

namespace Requeceptor.Services.RequestLoggers;

public class DatabaseRequestLoggerService : IRequestLoggerService
{
    private readonly DatabaseContext _db;

    public DatabaseRequestLoggerService(
        DatabaseContext db
        )
    {
        _db = db;
    }

    public async Task SaveAsync(RequestRecord request)
    {
        _db.Requests.Add(request);
        await _db.SaveChangesAsync();
    }
}
