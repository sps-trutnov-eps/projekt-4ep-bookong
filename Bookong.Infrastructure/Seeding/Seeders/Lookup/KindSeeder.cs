using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bookong.Domain.Entities;
using Bookong.Infrastructure.Data;

namespace Bookong.Infrastructure.Seeding.Seeders.Lookup;

public class KindSeeder : ISeeder
{
    public string Name => "Kinds";
    public int Order => 100;
    public bool IsDemoData => false;

    // Only three kinds are used: Epika, Lyrika, Drama
    private static readonly string[] Defaults = new[]
    {
        "Epika", "Lyrika", "Drama"
    };

    public async Task SeedAsync(BookongDbContext db, CancellationToken cancellationToken = default)
    {
        foreach (var name in Defaults)
        {
            if (!db.Kinds.Any(k => k.Name == name))
            {
                db.Kinds.Add(new Kind { PublicId = Guid.NewGuid(), Name = name });
            }
        }
        await db.SaveChangesAsync(cancellationToken);
    }
}
