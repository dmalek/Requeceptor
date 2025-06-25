using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Requeceptor.Domain;
using Requeceptor.Services.Persistence;
using System.Net;
using System.Text.RegularExpressions;

namespace Requeceptor.Services.Responses;

public class ResponseFactory : IResponseFactory
{
    private readonly IPersistenceService _persistence;

    public ResponseFactory(
        IPersistenceService persistence
        )
    {
        _persistence = persistence;
    }

    public async Task<IActionResult> CreateResponseAsync(HttpRequest request)
    {
        var path = request.Path.Value ?? "/";
        var query = request.QueryString.HasValue ? request.QueryString.Value : "";

        var allRules = await _persistence.Rules
            .Where( x=> x.Active)
            .ToListAsync();

        var matchingRule = allRules.FirstOrDefault(rule =>
            IsWildcardMatch(path, rule.Path) &&
            (string.IsNullOrEmpty(rule.QueryString) || IsWildcardMatch(query, rule.QueryString))
        );


        if (matchingRule == null)
            return new NotFoundResult();

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
        if (int.TryParse(status, out statusCode))
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