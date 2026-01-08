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

// Application Services
builder.Services.AddScoped<IBookQrCodeImageGenerationService, BookQrCodeImageGenerationService>();

// Application UseCases, Queries
builder.Services.AddScoped<IGetAvailableBooksQuery, GetAvailableBooksQuery>();
builder.Services.AddScoped<IGetMaturitaBookSelectionQuery, GetMaturitaBookSelectionQuery>();
builder.Services.AddScoped<ISelectMaturitaBookUseCase, SelectMaturitaBookUseCase>();
builder.Services.AddScoped<IDeselectMaturitaBookUseCase, DeselectMaturitaBookUseCase>();
builder.Services.AddScoped<IGetLibraryStatisticsQuery, GetLibraryStatisticsQuery>();
builder.Services.AddScoped<IExportBooksQuery, ExportBooksQuery>();
builder.Services.AddScoped<IExportBookQrCodesQuery, ExportBookQrCodesQuery>();
builder.Services.AddScoped<IGetAllBookLoansQuery, GetAllBookLoansQuery>();
builder.Services.AddScoped<IGetUserBookLoansQuery, GetUserBookLoansQuery>();
builder.Services.AddScoped<IGetBookLoanStatusQuery, GetBookLoanStatusQuery>();
builder.Services.AddScoped<ICreateBookLoanUseCase, CreateBookLoanUseCase>();
builder.Services.AddScoped<IReturnBookUseCase, ReturnBookUseCase>();
builder.Services.AddScoped<IProlongBookLoanUseCase, ProlongBookLoanUseCase>();

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
