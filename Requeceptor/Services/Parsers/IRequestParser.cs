using Requeceptor.Domain;

namespace Requeceptor.Services.Parsers;

public interface IRequestParser
{
    RequestFormat Format { get; }
    bool CanParse(string? contentType, string? body);
    string? GetActionName(HttpRequest request, string body);
}
