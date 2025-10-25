using System;
using Bookong.Application.UseCases.Commands;
using Bookong.Application.UseCases.Commands.Interfaces;
using Bookong.Application.UseCases.Queries;
using Bookong.Application.UseCases.Queries.Interfaces;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;
using Bookong.Infrastructure.Data;
using Bookong.Infrastructure.Services;
using Bookong.Web.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// Připojení k databázi
builder.Services.AddDbContext<BookongDbContext>(options =>
 options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Unit of Work a Queries
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IGetAvailableBooksQuery, GetAvailableBooksQuery>();
builder.Services.AddScoped<IAddBookToMaturitaSelectionCommand, AddBookToMaturitaSelectionCommand>();
builder.Services.AddScoped<IGetMaturitaBookSelectionQuery, GetMaturitaBookSelectionQuery>();
builder.Services.AddScoped<IRemoveBookFromMaturitaSelectionCommand, RemoveBookFromMaturitaSelectionCommand>();

// Blazor komponenty
builder.Services.AddRazorComponents()
 .AddInteractiveServerComponents();

var app = builder.Build();

// Seed only in Development
if (app.Environment.IsDevelopment())
{
 using (var scope = app.Services.CreateScope())
 {
 var db = scope.ServiceProvider.GetRequiredService<BookongDbContext>();
 if (!db.Users.Any())
 {
 db.Users.Add(new User
 {
 PublicId = Guid.NewGuid(),
 SamAccountName = "devuser",
 ObjectGuid = Guid.NewGuid(),
 Name = "Dev User",
 Email = "dev@example.com",
 OrganizationalUnit = "OU=Users"
 });
 db.SaveChanges();
 }
 }
}
else
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