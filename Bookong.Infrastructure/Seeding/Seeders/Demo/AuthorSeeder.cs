using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bookong.Domain.Entities;
using Bookong.Infrastructure.Data;

namespace Bookong.Infrastructure.Seeding.Seeders.Demo;

public class AuthorSeeder : ISeeder
{
    public string Name => "Demo Authors";
    public int Order => 200;
    public bool IsDemoData => true;

    private static readonly (string FullName, int BirthYear)[] Authors = new[]
    {
        ("Karel Čapek", 1890),
        ("Alois Jirásek", 1851),
        ("Bohumil Hrabal", 1914),
        ("Franz Kafka", 1883),
        ("Milan Kundera", 1929),
    };

    public async Task SeedAsync(BookongDbContext db, CancellationToken cancellationToken = default)
    {
        foreach (var (full, birth) in Authors)
        {
            var parts = full.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var first = parts.First();
            var last = parts.Length > 1 ? parts.Last() : string.Empty;
            var middle = parts.Length > 2 ? string.Join(' ', parts.Skip(1).Take(parts.Length - 2)) : string.Empty;

            if (!db.Authors.Any(a => a.FirstName == first && a.MiddleName == middle && a.LastName == last))
            {
                db.Authors.Add(new Author
                {
                    PublicId = Guid.NewGuid(),
                    FirstName = first,
                    MiddleName = middle,
                    LastName = last,
                    DateOfBirth = new DateTime(birth, 1, 1)
                });
            }
        }
        await db.SaveChangesAsync(cancellationToken);
    }
}
