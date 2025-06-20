using Microsoft.AspNetCore.Mvc;
using Requeceptor.Domain;
using Requeceptor.Services;
using Requeceptor.Services.Parsers;
using Requeceptor.Services.Persistence;

namespace Requeceptor.Controllers;

[ApiController]
//[Route("{*path}")] // Hvatamo sve rute
[Route("r/{*path}")]// hvata sve unutar /r/*/*
public class ReceptorController : ControllerBase
{
    private readonly IPersistenceService _requestLoggerService;
    private readonly IEnumerable<IRequestParser> _parsers;
    private readonly ILogger<ReceptorController> _logger;

    public ReceptorController(
        ILogger<ReceptorController> logger,
        IEnumerable<IRequestParser> parsers,
        IPersistenceService requestLoggerService
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
        string _soapOkResponse = @"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
  <soap:Body>
    <LogResponse xmlns=""http://tempuri.org/"">
      <Result>OK</Result>
    </LogResponse>
  </soap:Body>
</soap:Envelope>";

        return Task.FromResult<IActionResult>(Content(_soapOkResponse, "text/xml"));
        //return Task.FromResult<IActionResult>(Ok($"XML primljen. SOAP metoda: {methodName ?? "Nepoznata"}"));
    }
}
