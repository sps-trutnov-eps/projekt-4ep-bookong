using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bookong.Domain.Entities;
using Bookong.Infrastructure.Data;

namespace Bookong.Infrastructure.Seeding.Seeders.Lookup;

public class GenreSeeder : ISeeder
{
    public string Name => "Genres";
    public int Order => 110;
    public bool IsDemoData => false;

    // Representative genres used in the dataset
    private static readonly string[] Defaults = new[]
    {
        "Román", "Novela", "Povídka", "Románová kronika", "Historický román", "Soubor povídek", "Soubor pověstí", "Drama"
    };

    public async Task SeedAsync(BookongDbContext db, CancellationToken cancellationToken = default)
    {
        foreach (var name in Defaults)
        {
            if (!db.Genres.Any(k => k.Name == name))
            {
                db.Genres.Add(new Genre { PublicId = Guid.NewGuid(), Name = name });
            }
        }
        await db.SaveChangesAsync(cancellationToken);
    }
}
