using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Requeceptor.Domain;
using Requeceptor.Services.Persistence;

namespace Requeceptor.Components.Pages;

public partial class Requests
{
    private IQueryable<RequestRecord>? _requests = null;
    private IList<RequestRecord> _selectedRequests;
    private RequestRecord? _selectedRequest => _selectedRequests?.FirstOrDefault();

    [Inject]
    private DatabaseContext DbContext{ get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await LoadRequests();
    }

    private async Task LoadRequests()
    {
        _requests = DbContext.Requests
            .AsNoTracking()
            .OrderByDescending(x => x.ReceivedAt);            

        await Task.CompletedTask;
        StateHasChanged();
    }
}

