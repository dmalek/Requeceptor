using Requeceptor;
using Requeceptor.Components;

var builder = WebApplication.CreateBuilder(args);

// Uèitavanje konfiguracije s environment-specific json
var environment = builder.Environment.EnvironmentName;
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile($"appsettings.{environment}.json", optional: false, reloadOnChange: true);

builder.Services.AddControllers();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRequeceptor(builder.Configuration);

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

app.InitializePersistence(); // Inicijalizacija baze podataka

app.MapRequeceptorRoute();
app.MapControllers();
app.Run();
