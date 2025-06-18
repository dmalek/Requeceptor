using Microsoft.AspNetCore.Mvc;
using Requeceptor.Domain;
using Requeceptor.Services;
using Requeceptor.Services.Parsers;
using Requeceptor.Services.RequestLoggers;

namespace Requeceptor.Controllers;

[ApiController]
//[Route("{*path}")] // Hvatamo sve rute
[Route("r/{project}/{*path}")]// hvata sve unutar /r/*/*
public class ReceptorController : ControllerBase
{
    private readonly IRequestLoggerService _requestLoggerService;
    private readonly IEnumerable<IRequestParser> _parsers;
    private readonly ILogger<ReceptorController> _logger;

    public ReceptorController(
        ILogger<ReceptorController> logger,
        IEnumerable<IRequestParser> parsers,
        IRequestLoggerService requestLoggerService
        )
    {
        _logger = logger;
        _parsers = parsers;
        _requestLoggerService = requestLoggerService;
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

        if (string.IsNullOrEmpty(receptorRequest.Project))
        {
            return BadRequest("Unknown project.");
        }

        await _requestLoggerService.SaveAsync(receptorRequest.ToRequestRecord());

        switch (receptorRequest.RequestFormat)
        {
            case RequestFormat.Json:
                return await HandleJson(receptorRequest.ActionName);
            case RequestFormat.Xml:
                return await HandleXml(receptorRequest.ActionName);
            default:
                return await Task.FromResult<IActionResult>(Ok()); ;
        }
    }


    private Task<IActionResult> HandleJson(string? methodName)
    {
        return Task.FromResult<IActionResult>(Ok("JSON primljen."));
    }

    private Task<IActionResult> HandleXml(string? methodName)
    {
        return Task.FromResult<IActionResult>(Ok($"XML primljen. SOAP metoda: {methodName ?? "Nepoznata"}"));
    }
}
