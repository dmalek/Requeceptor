using Radzen;
using Requeceptor;
using Requeceptor.Components;

var builder = WebApplication.CreateBuilder(args);

// Uèitavanje konfiguracije s environment-specific json
var environment = builder.Environment.EnvironmentName;
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile($"appsettings.{environment}.json", optional: false, reloadOnChange: true);

builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddRadzenComponents();

builder.Services.AddRequeceptor(config => builder.Configuration.GetSection("RouteOptions").Bind(config));
builder.Services.UseRequeceptorPersistence(config => builder.Configuration.GetSection("Database").Bind(config));


var app = builder.Build();

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

app.InitializeRequeceptorPersistence();
app.MapRequeceptorController();

app.Run();
