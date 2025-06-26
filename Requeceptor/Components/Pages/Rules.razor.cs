using Microsoft.AspNetCore.Components;
using Requeceptor.Domain;
using Requeceptor.Services.Persistence;

namespace Requeceptor.Components.Pages;

public partial class Rules
{
    private IQueryable<RuleRecord>? _rules = null;
    private IList<RuleRecord> _selectedRules;

    [Inject]
    private IPersistenceService? PersistenceService { get; set; }


    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await LoadRules();
    }


    private async Task LoadRules()
    {
        if (PersistenceService == null)
        {
            return;
        }
        _rules = PersistenceService.Rules;

        await Task.CompletedTask;
        StateHasChanged();
    }

}
