using Microsoft.AspNetCore.Mvc;
using Requeceptor.Domain;

namespace Requeceptor.Services.Responses;

public interface IResponseFactory
{
    Task<ContentResult> CreateResponseAsync(HttpRequest request, RequestFormat requestFormat);
}