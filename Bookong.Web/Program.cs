using Bookong.Infrastructure.Data;
using Bookong.Web.Components;
using Microsoft.EntityFrameworkCore;
using Bookong.Domain.Interfaces;
using Bookong.Infrastructure.Services;
using Bookong.Application.UseCases.Queries.Interfaces;
using Bookong.Application.UseCases.Queries;
using Bookong.Application.Services.Interfaces;
using Bookong.Web.Services;
using Bookong.Application.UseCases.Interfaces;
using Bookong.Application.UseCases;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BookongDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Application UseCases, Queries
builder.Services.AddScoped<IGetAvailableBooksQuery, GetAvailableBooksQuery>();

// Session services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ISessionService, SessionService>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


builder.Services.AddScoped<ILoginUserUseCase, LoginUserUseCase>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseSession();

// Development only middleware to set a test user in session
if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<TestUserMiddleware>();
}

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
