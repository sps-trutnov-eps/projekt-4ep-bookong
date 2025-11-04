using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bookong.Domain.Entities;
using Bookong.Infrastructure.Data;

namespace Bookong.Infrastructure.Seeding.Seeders.Demo;

public class PublisherSeeder : ISeeder
{
    public string Name => "Demo Publishers";
    public int Order => 205;
    public bool IsDemoData => true;

    private static readonly string[] Publishers =
    {
        "Èeskoslovenský spisovatel",
        "Mladá fronta",
        "Odeon"
    };

    public async Task SeedAsync(BookongDbContext db, CancellationToken cancellationToken = default)
    {
        foreach (var name in Publishers)
        {
            if (!db.Publishers.Any(p => p.Name == name))
            {
                db.Publishers.Add(new Publisher
                {
                    PublicId = Guid.NewGuid(),
                    Name = name
                });
            }
        }
        await db.SaveChangesAsync(cancellationToken);
    }
}
