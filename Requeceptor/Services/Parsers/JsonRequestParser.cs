using Requeceptor.Domain;

namespace Requeceptor.Services.Parsers;

public class JsonRequestParser : IRequestParser
{
    public RequestFormat Format => RequestFormat.Json;

    public bool CanParse(string? contentType, string? body)
    {
        if (contentType?.Contains("json") == true) return true;
        var trimmed = body?.TrimStart();
        return trimmed?.StartsWith("{") == true || trimmed?.StartsWith("[") == true;
    }

    public string? GetActionName(HttpRequest request, string body)
    {
        var segments = request.Path.ToString().Split('/', StringSplitOptions.RemoveEmptyEntries);
        return segments.Length > 0 ? segments[^1] : null;
    }
}