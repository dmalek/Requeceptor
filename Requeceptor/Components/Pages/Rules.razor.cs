using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Requeceptor.Domain;
using Requeceptor.Services.Persistence;

namespace Requeceptor.Components.Pages;

public partial class Rules
{
    private IQueryable<RuleRecord>? _rules = null;
    private IList<RuleRecord> _selectedRules;

    [Inject]
    private DatabaseContext? Database { get; set; }


    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await LoadRules();
    }




    private async Task LoadRules()
    {
        _rules = Database.Rules
            .AsNoTracking();

        await Task.CompletedTask;
    }

}
