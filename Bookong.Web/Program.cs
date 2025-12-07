using Bookong.Infrastructure.Data;
using Bookong.Web.Components;
using Microsoft.EntityFrameworkCore;
using Bookong.Domain.Interfaces;
using Bookong.Infrastructure.Services;
using Bookong.Application.UseCases.Queries.Interfaces;
using Bookong.Application.UseCases.Queries;
using Bookong.Application.UseCases.Interfaces;
using Bookong.Application.UseCases;
using Bookong.Application.Services.Interfaces;
using Bookong.Web.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BookongDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Application use cases and queries
builder.Services.AddScoped<IGetAvailableBooksQuery, GetAvailableBooksQuery>();
builder.Services.AddScoped<IGetAllBooksQuery, GetAllBooksQuery>();
builder.Services.AddScoped<IGetMaturitaBookSelectionQuery, GetMaturitaBookSelectionQuery>();
builder.Services.AddScoped<ISelectMaturitaBookUseCase, SelectMaturitaBookUseCase>();
builder.Services.AddScoped<IDeselectMaturitaBookUseCase, DeselectMaturitaBookUseCase>();
builder.Services.AddScoped<IGetLibraryStatisticsQuery, GetLibraryStatisticsQuery>();

// Register new query implementation required by adapter/wrapper
builder.Services.AddScoped<IGetMaturitaBooksQuery, GetMaturitaBooksQuery>();

// Queries
// register implementation that matches IGetAvailableMaturitaBooksQuery (wrapper/adapter depends on IGetMaturitaBooksQuery)
builder.Services.AddScoped<IGetAvailableMaturitaBooksQuery, GetAvailableMaturitaBooksQuery>();

// Use cases
builder.Services.AddScoped<IManageMaturitaBookAvailabilityUseCase, ManageMaturitaBookAvailabilityUseCase>();

// Session services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ISessionService, SessionService>();

// Add components to the DI container and enable interactive server components.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // Use a central error handler and HSTS in production.
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseSession();

// Development-only middleware that seeds a test user into the session
if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<TestUserMiddleware>();
}

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();