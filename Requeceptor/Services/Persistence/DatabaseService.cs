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

    public Task<List<string>> Projects()
    {
        return _db.Requests
            .Where( x => !string.IsNullOrEmpty(x.Project))  
            .Select( x => x.Project)
            .Distinct()
            .OrderBy(x => x)
            .ToListAsync();
    }

    public Task<List<string>> Hosts()
    {
        return _db.Requests
            .Where(x => !string.IsNullOrEmpty(x.Host))
            .Select(x => x.Host)
            .Distinct()
            .OrderBy(x => x)
            .ToListAsync();
    }
}
