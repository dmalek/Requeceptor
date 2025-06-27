using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;

namespace Requeceptor.Components;

public partial class App : ComponentBase
{
    [Inject]
    private IOptions<RequeceptorOptions> Configuration { get; set; } = default!;

    private string GetBaseUrl()
    {        
        return Configuration.Value?.BaseRoute ?? "/";
    }
}
