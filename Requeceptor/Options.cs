namespace Requeceptor;

public class DatabaseOptions
{
    public string? Provider { get; set; }
    public string? ConnectionId { get; set; }
    public Dictionary<string, string> ConnectionStrings { get; set; } = new();
}

public class RequeceptorOptions
{
    public string? BaseRoute { get; set; }
    public string? ApiRoute { get; set; }
}