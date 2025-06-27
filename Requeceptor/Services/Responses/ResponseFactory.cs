using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Requeceptor.Domain;
using Requeceptor.Services.Persistence;
using System.Net;
using System.Text.RegularExpressions;

namespace Requeceptor.Services.Responses;

public class ResponseFactory : IResponseFactory
{
    private readonly DatabaseContext _database;

    public ResponseFactory(
        DatabaseContext database
        )
    {
        _database = database ?? throw new ArgumentNullException(nameof(database));
    }

    public async Task<ContentResult> CreateResponseAsync(HttpRequest request, RequestFormat requestFormat)
    {
        var path = request.Path.Value ?? "/";
        var query = request.QueryString.HasValue ? request.QueryString.Value : "";
        var method = request.Method.ToUpperInvariant();

        RuleRecord? matchingRule = await GetMatchingRule(path, query, method);

        // Ako nema odgovarajućeg pravila, vratimo default odgovor
        if (matchingRule == null)
        {
            return GetDefaultOkResult(requestFormat);
        }

        var statusCode = TryParseStatus(matchingRule.ResponseStatus, out var code) ? code : 200;
        var content = matchingRule.ResponseBody ?? "";
        var contentType = matchingRule.ResponseContentType ?? "application/json";

        return new ContentResult
        {
            Content = content,
            StatusCode = statusCode,
            ContentType = contentType
        };
    }

    private async Task<RuleRecord?> GetMatchingRule(string path, string query, string method)
    {
        var allRules = await _database.Rules            
            .AsNoTracking()
            .Where(x => x.Enabled)
            .ToListAsync();

        var rulesForAction = allRules.Where(rule =>
                        IsWildcardMatch(path, rule.Path) &&
                        (string.IsNullOrEmpty(rule.QueryString) || IsWildcardMatch(query, rule.QueryString))
                        );

        var universalRule = rulesForAction.FirstOrDefault(x => x.Method == "*");
        var methodRule = rulesForAction.FirstOrDefault( x => x.Method.Equals(method, StringComparison.OrdinalIgnoreCase));

        return methodRule ?? universalRule;
    }

    private ContentResult GetDefaultOkResult(RequestFormat requestFormat)
    {
        switch (requestFormat)
        {
            case RequestFormat.Xml:
                return new ContentResult
                {
                    StatusCode = 200,
                    Content = @"<?xml version=""1.0"" encoding=""utf-8""?><soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""><soap:Body /></soap:Envelope>",
                    ContentType = "text/xml; charset=utf-8"
                };
            default:
                return new ContentResult
                {
                    StatusCode = 200,
                    Content = "OK",
                    ContentType = "text/plain"
                };
        }
    }

    private static bool IsWildcardMatch(string input, string pattern)
    {
        if (string.IsNullOrEmpty(pattern)) return false;

        // Escape korisnički unos i zamijeni wildcards
        var regexPattern = "^" + Regex.Escape(pattern)
            .Replace("\\*", ".*")
            .Replace("\\?", ".") + "$";

        return Regex.IsMatch(input, regexPattern, RegexOptions.IgnoreCase);
    }


    private bool TryParseStatus(string? status, out int statusCode)
    {        
        if (int.TryParse(status?.Split(" ", StringSplitOptions.RemoveEmptyEntries).FirstOrDefault(), out statusCode))
            return true;

        // dodatno: status može biti "OK", "BadRequest" itd.
        if (Enum.TryParse<HttpStatusCode>(status ?? "", true, out var code))
        {
            statusCode = (int)code;
            return true;
        }

        statusCode = 200;
        return false;
    }
}