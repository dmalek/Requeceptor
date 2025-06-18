using Microsoft.EntityFrameworkCore;
using Requeceptor;
using Requeceptor.Components;
using Requeceptor.Services.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Uèitavanje konfiguracije s environment-specific json
var environment = builder.Environment.EnvironmentName;
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile($"appsettings.{environment}.json", optional: false, reloadOnChange: true);

// 1. Registracija servisa
builder.Services.AddControllers(); // Za API kontrolere s [ApiController]
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRequeceptor(builder.Configuration);

var app = builder.Build();

// 2. Middleware konfiguracija
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

// app.UseHttpsRedirection(); // Ako koristiš HTTPS, odkomentiraj
app.UseStaticFiles();

app.UseRouting();

app.UseAntiforgery();

// 3. Mapiranje ruta i Blazor komponente
app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.InitializePersistence(); // Inicijalizacija baze podataka

app.MapControllers();
app.MapRequeceptorRoute();
app.Run();
