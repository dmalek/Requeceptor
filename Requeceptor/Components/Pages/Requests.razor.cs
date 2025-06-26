using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Requeceptor.Core;
using Requeceptor.Domain;
using Requeceptor.Services.Persistence;
using System.Text;
using System.Text.Json;
using System.Xml;

namespace Requeceptor.Components.Pages;

public partial class Requests
{
    private IQueryable<RequestRecord>? _requests = null;
    private IList<RequestRecord> _selectedRequests;

    [Inject]
    private IPersistenceService? PersistenceService { get; set; }

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
        var query = PersistenceService.Requests;
        query = query.Case(!string.IsNullOrEmpty(Path), x => EF.Functions.Like(x.Path, Path.Replace("*", "%")));
        query = query.Case(!string.IsNullOrEmpty(Action), x => EF.Functions.Like(x.Action, Action.Replace("*", "%")));
        query = query.Case(!string.IsNullOrEmpty(Query), x => x.QueryString.Contains(Query.Replace("*", "%")));

        _requests = query
            .OrderByDescending(x => x.ReceivedAt);
        //.ToPaginatedList(PageIndex, PageSize);

        await Task.CompletedTask;
        StateHasChanged();
    }

    private string FormatBody(string? body)
    {
        if (string.IsNullOrWhiteSpace(body))
            return string.Empty;

        try
        {
            body = body.Trim();

            if (body.StartsWith("{") || body.StartsWith("["))
            {
                // Pretpostavi JSON
                using var doc = JsonDocument.Parse(body);
                return JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true });
            }
            else if (body.StartsWith("<"))
            {
                // Pretpostavi XML
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(body);

                var sb = new StringBuilder();
                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "  ",
                    NewLineChars = "\n",
                    NewLineHandling = NewLineHandling.Replace
                };

                using var writer = XmlWriter.Create(sb, settings);
                xmlDoc.Save(writer);
                return sb.ToString();
            }

            return body; // fallback ako nije prepoznat
        }
        catch
        {
            return body; // ako ne uspije parsiranje, prikaži kako je
        }
    }
}

