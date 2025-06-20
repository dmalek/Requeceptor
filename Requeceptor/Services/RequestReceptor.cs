using Requeceptor.Domain;
using Requeceptor.Services.Parsers;

namespace Requeceptor.Services;

public class RequestReceptor
{
    private readonly HttpContext _httpContext;
    private readonly RouteData _routeData;
    private readonly HttpRequest _request;
    private readonly IEnumerable<IRequestParser> _parsers;

    private string? _body;
    private IRequestParser? _parser;
    private string? _actionName;

    public string? Body => _body;
    public string? ActionName => _actionName;
    public RequestFormat RequestFormat => _parser?.Format ?? RequestFormat.Unknown;

    public RequestReceptor(HttpRequest request, RouteData routeData, HttpContext httpContext, IEnumerable<IRequestParser> parsers)
    {
        _request = request;
        _routeData = routeData;
        _httpContext = httpContext;
        _parsers = parsers;
        _request.EnableBuffering();
    }

    public async Task Inspect()
    {
        // Read the request body
        using var reader = new StreamReader(_request.Body, leaveOpen: true);
        _body = await reader.ReadToEndAsync();
        _request.Body.Position = 0;

        // Parse the request body
        _parser = _parsers.FirstOrDefault(p => p.CanParse(_request.ContentType, _body));
        if (_parser != null)
        {
            _actionName = _parser.GetActionName(_request, _body);
        }
    }

    public RequestRecord ToRequestRecord()
    {
        return new RequestRecord
        {
            Scheme = _httpContext.Request.Scheme,
            Host = _httpContext.Request.Host.Value,
            Path = _httpContext.Request.Path,
            QueryString = _httpContext.Request.QueryString.ToString(),
            Action = _actionName,
            Method = _httpContext.Request.Method,
            Protocol = _httpContext.Request.Protocol,
            ContentType = _request.ContentType,
            ContentLength = _request.ContentLength,
            Headers = string.Join("\n", _httpContext.Request.Headers.Select(h => $"{h.Key}: {h.Value}")),
            Cookies = string.Join("\n", _httpContext.Request.Cookies.Select(c => $"{c.Key}: {c.Value}")),
            Body = _body,
            RemoteIpAddress = _httpContext.Connection.RemoteIpAddress?.ToString(),
            LocalIpAddress = _httpContext.Connection.LocalIpAddress?.ToString()
        };
    }
}
