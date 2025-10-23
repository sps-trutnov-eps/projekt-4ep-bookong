using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Bookong.Infrastructure.Data;

namespace Bookong.Infrastructure.Seeding;

public class SeedRunner
{
    private readonly ILogger<SeedRunner> _logger;

    public SeedRunner(ILogger<SeedRunner> logger)
    {
        _logger = logger;
    }

    public async Task RunAsync(BookongDbContext db, ISeeder[] seeders, bool includeDemoData, CancellationToken ct = default)
    {
        var ordered = seeders
            .Where(s => includeDemoData || !s.IsDemoData)
            .OrderBy(s => s.Order)
            .ToArray();

        foreach (var seeder in ordered)
        {
            try
            {
                _logger.LogInformation("Seeding {Seeder}...", seeder.Name);
                await seeder.SeedAsync(db, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Seeder {Seeder} failed", seeder.Name);
                throw;
            }
        }
    }
}
