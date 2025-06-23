using Microsoft.AspNetCore.Mvc;

namespace Requeceptor.Services.Responses;

public interface IResponseFactory
{
    Task<IActionResult> CreateResponseAsync(HttpRequest request);
}