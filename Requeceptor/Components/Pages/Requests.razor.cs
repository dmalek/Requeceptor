using Microsoft.AspNetCore.Components;
using Requeceptor.Core;
using Requeceptor.Domain;
using Requeceptor.Services.Persistence;

namespace Requeceptor.Components.Pages;

public partial class Requests
{
    private PaginatedList<RequestRecord>? _requests = null;    
    private List<string> _projects = new();
    private List<string> _hosts = new();

    [Inject]
    private IPersistenceService? PersistenceService { get; set; }

    private string? Project { get; set; }
    private string? Host { get; set; }
    private string? Path { get; set; }
    private string? Action { get; set; }
    private string? Query { get; set; }
    private int PageIndex { get; set; } = 0;
    private int PageSize { get; set; } = 10;

    protected override void OnInitialized()
    {
        base.OnInitialized();
       
    }

    protected override async Task OnInitializedAsync()
    {       
        await base.OnInitializedAsync();
        await LoadRequests();
    }

    private async Task LoadRequests()
    {
        if (PersistenceService == null)
        {
            return;
        }
        var query = PersistenceService.Requests();
        query = query.Case(!string.IsNullOrEmpty(Project), x => x.Project.Contains(Project));
        query = query.Case(!string.IsNullOrEmpty(Host), x => x.Host.Contains(Host));
        query = query.Case(!string.IsNullOrEmpty(Path), x => x.Path.Contains(Path));
        query = query.Case(!string.IsNullOrEmpty(Action), x => x.Action.Contains(Action));
        query = query.Case(!string.IsNullOrEmpty(Query), x => x.QueryString.Contains(Query));

        _requests = query
            .OrderByDescending( x => x.ReceivedAt)
            .ToPaginatedList(PageIndex, PageSize);

        _projects = await PersistenceService.Projects();
        _hosts = await PersistenceService.Hosts();
        
        StateHasChanged();
    }
}

