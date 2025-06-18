using Requeceptor.Domain;

namespace Requeceptor.Services.Persistence;

public interface IPersistenceService
{
    Task SaveAsync(RequestRecord request);

    IQueryable<RequestRecord> Requests();
    Task<List<string>> Projects();
    Task<List<string>> Hosts();
}