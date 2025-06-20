using Requeceptor.Domain;

namespace Requeceptor.Services.Persistence;

public interface IPersistenceService
{
    Task SaveAsync(RequestRecord request);

    IQueryable<RequestRecord> Requests();
}