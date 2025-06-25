using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Requeceptor;
using Requeceptor.Components;
using Requeceptor.Services.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Load environment-specific config
var environment = builder.Environment.EnvironmentName;
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile($"appsettings.{environment}.json", optional: false, reloadOnChange: true);

// Services
builder.Services.AddControllers();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRequeceptor(builder.Configuration);
builder.Services.AddRazorPages();
builder.Services.AddMudServices();
builder.Services.AddAuthorizationCore();

var app = builder.Build();

app.MapStaticAssets();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAntiforgery();

app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.InitializePersistence();
app.MapRequeceptorRoute();
app.MapControllers();

app.Run();
