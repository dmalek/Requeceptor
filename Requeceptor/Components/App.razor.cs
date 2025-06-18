using Microsoft.AspNetCore.Components;

namespace Requeceptor.Components;

public partial class App : ComponentBase
{
    [Inject]
    private IConfiguration Configuration { get; set; } = default!;

    private string GetBaseUrl()
    {
        var bu = Configuration["BaseUrl"];
        return string.IsNullOrWhiteSpace(bu) ? "/" : bu;
    }
}
