using System.CommandLine;
using System.CommandLine.Invocation;
using Bookong.Infrastructure.Data;
using Bookong.Infrastructure.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var root = new RootCommand("Bookong CLI tool");

var connOpt = new Option<string?>(name: "--connection", description: "Connection string to the SQL Server database. If not provided, defaults to LocalDB.");

// migrate command
var migrateCmd = new Command("migrate", "Run database migrations only (no seeding)")
{
    connOpt
};

migrateCmd.SetHandler(async (InvocationContext ctx) =>
{
    var connection = ctx.ParseResult.GetValueForOption(connOpt);

    // Determine connection string
    var connectionString = !string.IsNullOrWhiteSpace(connection)
        ? connection
        : Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
          ?? "Server=(localdb)\\mssqllocaldb;Database=BookongDb;Trusted_Connection=True;";

    // Build services
    var services = new ServiceCollection();
    services.AddLogging(b =>
    {
        b.AddConsole();
        b.SetMinimumLevel(LogLevel.Information);
    });
    services.AddDbContext<BookongDbContext>(opt => opt.UseSqlServer(connectionString));

    await using var provider = services.BuildServiceProvider();
    using var scope = provider.CreateScope();

    var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Migrate");
    var db = scope.ServiceProvider.GetRequiredService<BookongDbContext>();

    logger.LogInformation("Running database migrations...");
    await db.Database.MigrateAsync(ctx.GetCancellationToken());
    logger.LogInformation("Migrations complete.");
});

// seed command
var demoOpt = new Option<bool>(name: "--demo", description: "Include demo data (authors, warehouses, books)", getDefaultValue: () => true);
var resetOpt = new Option<bool>(name: "--reset", description: "Soft reset demo data before seeding", getDefaultValue: () => false);

var seedCmd = new Command("seed", "Run database seeders (lookups and optionally demo data)")
{
    demoOpt,
    resetOpt,
    connOpt
};

seedCmd.SetHandler(async (InvocationContext ctx) =>
{
    var demo = ctx.ParseResult.GetValueForOption(demoOpt);
    var reset = ctx.ParseResult.GetValueForOption(resetOpt);
    var connection = ctx.ParseResult.GetValueForOption(connOpt);

    // Determine connection string
    var connectionString = !string.IsNullOrWhiteSpace(connection)
        ? connection
        : Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
          ?? "Server=(localdb)\\mssqllocaldb;Database=BookongDb;Trusted_Connection=True;";

    // Build services
    var services = new ServiceCollection();
    services.AddLogging(b =>
    {
        b.AddConsole();
        b.SetMinimumLevel(LogLevel.Information);
    });
    services.AddDbContext<BookongDbContext>(opt => opt.UseSqlServer(connectionString));
    services.AddBookongSeeders();

    await using var provider = services.BuildServiceProvider();
    using var scope = provider.CreateScope();

    var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Seed");
    var db = scope.ServiceProvider.GetRequiredService<BookongDbContext>();
    var runner = scope.ServiceProvider.GetRequiredService<SeedRunner>();
    var seeders = scope.ServiceProvider.GetServices<ISeeder>().ToArray();

    logger.LogInformation("Migrating database...");
    await db.Database.MigrateAsync(ctx.GetCancellationToken());

    if (reset)
    {
        logger.LogWarning("Soft reset enabled: clearing demo-related tables...");
        db.Books.RemoveRange(db.Books);
        db.Authors.RemoveRange(db.Authors);
        db.Warehouses.RemoveRange(db.Warehouses);
        db.Addresses.RemoveRange(db.Addresses);
        db.Publishers.RemoveRange(db.Publishers);
        await db.SaveChangesAsync(ctx.GetCancellationToken());
    }

    logger.LogInformation("Running seeders. Include demo: {Demo}", demo);
    await runner.RunAsync(db, seeders, demo, ctx.GetCancellationToken());

    logger.LogInformation("Seeding complete.");
});

root.AddCommand(migrateCmd);
root.AddCommand(seedCmd);

return await root.InvokeAsync(args);
