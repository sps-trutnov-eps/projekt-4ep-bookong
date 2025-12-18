using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bookong.Domain.Entities;
using Bookong.Infrastructure.Data;

namespace Bookong.Infrastructure.Seeding.Seeders.Lookup;

public class PeriodSeeder : ISeeder
{
    public string Name => "Periods";
    public int Order => 120;
    public bool IsDemoData => false;

    private static readonly string[] Defaults = new[]
    {
        "Česká lit 20. a 21. st.",
        "Sv. a česká lit 19. st.",
        "Sv. lit 20. a 21. st."
    };

    public async Task SeedAsync(BookongDbContext db, CancellationToken cancellationToken = default)
    {
        foreach (var name in Defaults)
        {
            if (!db.Periods.Any(k => k.Name == name))
            {
                db.Periods.Add(new Period { PublicId = Guid.NewGuid(), Name = name });
            }
        }
        await db.SaveChangesAsync(cancellationToken);
    }
}
