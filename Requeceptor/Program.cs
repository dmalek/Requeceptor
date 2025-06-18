using Microsoft.EntityFrameworkCore;
using Requeceptor;
using Requeceptor.Components;
using Requeceptor.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Servisi
builder.Services.AddControllers(); // Dodaj ovo ako koristiš [ApiController]
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddRequeceptor();
var app = builder.Build();

// 2. Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting(); // VAŽNO: prije MapEndpoints()

app.UseAntiforgery();

// 3. Route mapping
app.MapRazorComponents<App>() // Blazor entry point
    .AddInteractiveServerRenderMode();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();

    endpoints.MapControllerRoute(
        name: "Receptor",
        pattern: "r/{*path}",
        defaults: new { controller = "Receptor", action = "CatchAll" });
});

app.Run();
