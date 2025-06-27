using Microsoft.AspNetCore.Components;
using Requeceptor.Domain;
using Requeceptor.Services.Persistence;
using System.Net;

namespace Requeceptor.Components.Pages;

public partial class RuleEdit
{
    [Inject]
    private DatabaseContext? _database { get; set; }

    [Parameter]
    public RuleRecord ViewModel { get; set; } = new();

    [Parameter]
    public EventCallback<RuleRecord> OnSubmit { get; set; }

    private List<string> _httpStatuses => Enum.GetValues(typeof(HttpStatusCode))
        .Cast<HttpStatusCode>()
        .Select(code => $"{(int)code} {code}")
        .Distinct()
        .ToList();

    private List<string> _methods => new List<string>
    {
        "*",
        "GET",
        "POST",
        "PUT",
        "PATCH",
        "DELETE",
        "HEAD",
        "OPTIONS",
        "TRACE",
        "CONNECT"
    };

    protected async Task Submit()
    {
        if (ViewModel.Id == 0)
        {
            _database.Add(ViewModel);
        }
        else
        {
            _database.Update(ViewModel);
        }

        await _database.SaveChangesAsync();  

        if (OnSubmit.HasDelegate)
        {
            await OnSubmit.InvokeAsync(ViewModel);
        }
    }

    protected async Task Cancel()
    {
        await _database.Entry(ViewModel).ReloadAsync();
    }
}
