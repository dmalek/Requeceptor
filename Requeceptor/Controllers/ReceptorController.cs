using Microsoft.AspNetCore.Mvc;
using Requeceptor.Domain;
using Requeceptor.Services.Parsers;
using Requeceptor.Services.Persistence;
using Requeceptor.Services.Requests;
using Requeceptor.Services.Responses;

namespace Requeceptor.Controllers;

[ApiController]
//[Route("{*path}")] // Hvatamo sve rute
[Route("r/{*path}")]// hvata sve unutar /r/*/*
public class ReceptorController : ControllerBase
{
    private readonly ILogger<ReceptorController> _logger;
    private readonly IPersistenceService _requestLoggerService;
    private readonly IEnumerable<IRequestParser> _parsers;
    private readonly IResponseFactory _responseFactory;

    public ReceptorController(
        ILogger<ReceptorController> logger,
        IEnumerable<IRequestParser> parsers,
        IPersistenceService requestLoggerService,
        IResponseFactory responseFactory
        )
    {
        _logger = logger;
        _parsers = parsers;
        _requestLoggerService = requestLoggerService;
        _responseFactory = responseFactory;
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

        var ruleResponse = await _responseFactory.CreateResponseAsync(Request);
        if (ruleResponse is not NotFoundResult)
            return ruleResponse;

        // fallback obrada
        switch (receptorRequest.RequestFormat)
        {
            case RequestFormat.Json:
                return await HandleJson();
            case RequestFormat.Xml:
                return await HandleXml();
            default:
                return Ok();
        }
    }


    private Task<IActionResult> HandleJson()
    {
        return Task.FromResult<IActionResult>(Ok("Ok"));
    }

    private Task<IActionResult> HandleXml()
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
    }
}
