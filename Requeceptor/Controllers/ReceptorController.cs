using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Requeceptor.Domain;
using Requeceptor.Services.Parsers;
using Requeceptor.Services.Persistence;
using Requeceptor.Services.Requests;
using Requeceptor.Services.Responses;
using System.Net;

namespace Requeceptor.Controllers;

public class ReceptorController : ControllerBase
{
    private readonly ILogger<ReceptorController> _logger;
    private readonly DatabaseContext _database;
    private readonly IEnumerable<IRequestParser> _parsers;
    private readonly IResponseFactory _responseFactory;
    private readonly IOptions<RequeceptorOptions> _options;

    public ReceptorController(
        ILogger<ReceptorController> logger,
        IEnumerable<IRequestParser> parsers,
        DatabaseContext database,
        IResponseFactory responseFactory,
        IOptions<RequeceptorOptions> options
        )
    {
        _logger = logger;
        _parsers = parsers;
        _database = database;
        _responseFactory = responseFactory;
        _options = options;
    }

    [HttpPost]
    [HttpGet]
    [HttpPut]
    [HttpDelete]
    [HttpPatch]
    [HttpHead]
    [HttpOptions]
    public async Task<IActionResult> CatchAll()
    {
        var receptorRequest = new RequestReceptor(Request, RouteData, HttpContext, _parsers);
        await receptorRequest.Inspect();
        var requestRecord = receptorRequest.ToRequestRecord();
        requestRecord.Path = PathWithoutRoot(requestRecord.Path);  
        
        ContentResult ruleResponse = await _responseFactory.CreateResponseAsync(receptorRequest.RequestFormat, requestRecord.Method, requestRecord.Path, requestRecord.QueryString);

        var statusCode = ruleResponse.StatusCode ?? 200;
        requestRecord.ResponseStatus = Enum.IsDefined(typeof(HttpStatusCode), statusCode)
            ? $"{statusCode} {((HttpStatusCode)statusCode)}"
            : $"{statusCode}";
        requestRecord.ResponseContentType = ruleResponse.ContentType;
        requestRecord.ResponseBody = ruleResponse.Content;

        _database.Requests.Add(requestRecord);
        await _database.SaveChangesAsync();

        return ruleResponse;
    }

    private string PathWithoutRoot(string path)
    {
        var apiRoute = _options?.Value?.ApiRoute ?? "/";    
        if (path.StartsWith(apiRoute, StringComparison.OrdinalIgnoreCase))
        {
            return path.Substring(apiRoute.Length);
        }
        return path;
    }
}
