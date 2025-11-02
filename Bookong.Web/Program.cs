using Bookong.Infrastructure.Data;
using Bookong.Web.Components;
using Microsoft.EntityFrameworkCore;
using Bookong.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Připojení databáze
builder.Services.AddDbContext<BookongDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrace služeb
builder.Services.AddScoped<Bookong.Web.Services.ITeachingMaterialsApi, Bookong.Web.Services.TeachingMaterialsApi>();

// Přidání controllerů pro API
builder.Services.AddControllers();

// Přidání Razor Components
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Konfigurace HTTP pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

// Mapování statických souborů
app.MapStaticAssets();

// Mapování Razor komponent
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Mapování controllerů (API)
app.MapControllers();

app.Run();
