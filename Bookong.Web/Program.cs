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

using Bookong.Application.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Bookong.Application.DTOs;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BookongDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Application UseCases, Queries
builder.Services.AddScoped<IGetAvailableBooksQuery, GetAvailableBooksQuery>();
builder.Services.AddScoped<IGetAllBooksQuery, GetAllBooksQuery>();
builder.Services.AddScoped<IGetMaturitaBookSelectionQuery, GetMaturitaBookSelectionQuery>();
builder.Services.AddScoped<IGetLibraryStatisticsQuery, GetLibraryStatisticsQuery>();
builder.Services.AddScoped<IExportBooksQuery, ExportBooksQuery>();
builder.Services.AddScoped<ILoginUserUseCase, LoginUserUseCase>();
builder.Services.AddScoped<ICompleteUserRegistrationUseCase, CompleteUserRegistrationUseCase>();
builder.Services.AddScoped<ISelectMaturitaBookUseCase, SelectMaturitaBookUseCase>();
builder.Services.AddScoped<IDeselectMaturitaBookUseCase, DeselectMaturitaBookUseCase>();
builder.Services.AddScoped<IGetLibraryStatisticsQuery, GetLibraryStatisticsQuery>();
// Queries
builder.Services.AddScoped<IGetMaturitaBooksQuery, GetMaturitaBooksQuery>();

// Use Cases
builder.Services.AddScoped<ICreateMaturitaBookUseCase, CreateMaturitaBookUseCase>();
builder.Services.AddScoped<IDeleteMaturitaBookUseCase, DeleteMaturitaBookUseCase>();

// Session services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddScoped<IActiveDirectoryService, MockActiveDirectoryService>();
}
else
{
    builder.Services.AddScoped<IActiveDirectoryService, ActiveDirectoryService>();
}


builder.Services.AddHttpContextAccessor();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/portalpage";
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<CookieAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CookieAuthenticationStateProvider>());
builder.Services.AddScoped<AuthJsInterop>();
builder.Services.AddHttpClient();

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
app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapPost("/auth/login", async (CookieAuthenticationStateProvider provider, LoginUserRequest request) =>
{
    var result = await provider.SignInAsync(request);

    if (result.Success || result.IsFirstTimeUser)
    {
        return Results.Ok(result);
    }

    return Results.BadRequest(result);
});

app.MapPost("/auth/register", async (CookieAuthenticationStateProvider provider, ICompleteUserRegistrationUseCase registrationUseCase, CompleteUserRegistrationRequest request) =>
{
    var response = await registrationUseCase.ExecuteAsync(request);

    if (!response.Success || response.UserId == Guid.Empty)
    {
        return Results.BadRequest(response);
    }

    var user = new UserSessionInfo
    {
        UserId = response.UserId,
        Username = response.Username ?? request.Username,
        Name = response.Name ?? request.Name,
        Email = response.Email ?? $"{request.Username}@spstrutnov.cz",
        OrganizationalUnit = response.OrganizationalUnit ?? "Default"
    };

    await provider.SignInKnownUserAsync(user);

    return Results.Ok(response);
});

app.MapPost("/auth/logout", async (CookieAuthenticationStateProvider provider) =>
{
    await provider.LogoutAsync();
    return Results.Ok();
});

app.Run();
