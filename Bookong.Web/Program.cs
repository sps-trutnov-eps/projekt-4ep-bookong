using Bookong.Application.UseCases.Queries;
using Bookong.Application.UseCases.Queries.Interfaces;
using Bookong.Domain.Interfaces;
using Bookong.Infrastructure.Data;
using Bookong.Infrastructure.Services;
using Bookong.Web.Components;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Připojení k databázi
builder.Services.AddDbContext<BookongDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Unit of Work a Queries
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IGetAvailableBooksQuery, GetAvailableBooksQuery>();

// Blazor komponenty
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();