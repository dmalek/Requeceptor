using Microsoft.EntityFrameworkCore;
using Requeceptor.Domain;

namespace Requeceptor.Services.Persistence;

public class DatabaseService : IPersistenceService
{
    private readonly DatabaseContext _db;

    public DatabaseService(
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

    public IQueryable<RequestRecord> Requests()
    {
        return _db.Requests;
    }
}
