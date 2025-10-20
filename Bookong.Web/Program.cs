using Bookong.Infrastructure.Data;
using Bookong.Web.Components;
using Microsoft.EntityFrameworkCore;
using Bookong.Domain.Interfaces;
using Bookong.Infrastructure.Services;
using Bookong.Application.UseCases.Queries.Interfaces;
using Bookong.Application.UseCases.Queries;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BookongDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Application UseCases, Queries
builder.Services.AddScoped<IGetAvailableBooksQuery, GetAvailableBooksQuery>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
