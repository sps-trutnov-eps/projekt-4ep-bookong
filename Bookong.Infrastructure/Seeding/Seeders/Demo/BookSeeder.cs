using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bookong.Domain.Entities;
using Bookong.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bookong.Infrastructure.Seeding.Seeders.Demo;

public class BookSeeder : ISeeder
{
    public string Name => "Demo Books";
    public int Order => 300;
    public bool IsDemoData => true;

    public async Task SeedAsync(BookongDbContext db, CancellationToken cancellationToken = default)
    {
        // Lookup helpers
        async Task<Author> A(string full)
        {
            var parts = full.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var first = parts.First();
            var last = parts.Length > 1 ? parts.Last() : string.Empty;
            var middle = parts.Length > 2 ? string.Join(' ', parts.Skip(1).Take(parts.Length - 2)) : string.Empty;
            return await db.Authors.FirstAsync(a => a.FirstName == first && a.MiddleName == middle && a.LastName == last, cancellationToken);
        }
        Task<Publisher?> P(string? name) => name is null
            ? Task.FromResult<Publisher?>(null)
            : db.Publishers.FirstOrDefaultAsync(p => p.Name == name, cancellationToken);
        Task<Genre> G(string name) => db.Genres.FirstAsync(g => g.Name == name, cancellationToken);
        Task<Kind> K(string name) => db.Kinds.FirstAsync(k => k.Name == name, cancellationToken);
        Task<Period> Pe(string name) => db.Periods.FirstAsync(p => p.Name == name, cancellationToken);

        static string MapKind(string input) => input switch
        {
            "Drama" => "Drama",
            "Lyrika" => "Lyrika",
            _ => "Epika"
        };

        async Task EnsureBook(string title, string author, string? publisher, string kind, string genre, ushort pages, string period, int year)
        {
            if (await db.Books.AnyAsync(b => b.Name == title, cancellationToken)) return;

            var book = new Book
            {
                PublicId = Guid.NewGuid(),
                Name = title,
                Author = await A(author),
                Publisher = await P(publisher),
                Genre = await G(genre),
                Kind = await K(MapKind(kind)),
                Period = await Pe(period),
                Pages = pages,
                Borrowable = true,
                DateRelease = new DateTime(year, 1, 1)
            };
            db.Books.Add(book);
            await db.SaveChangesAsync(cancellationToken);
        }

        // Čapek
        await EnsureBook("R.U.R.", "Karel Čapek", "Československý spisovatel", "Drama", "Drama", 96, "Česká lit 20. a 21. st.", 1920);
        await EnsureBook("Bílá nemoc", "Karel Čapek", "Československý spisovatel", "Drama", "Drama", 112, "Česká lit 20. a 21. st.", 1937);
        await EnsureBook("Matka", "Karel Čapek", "Československý spisovatel", "Drama", "Drama", 88, "Česká lit 20. a 21. st.", 1938);
        await EnsureBook("Krakatit", "Karel Čapek", "Odeon", "Epika", "Román", 256, "Česká lit 20. a 21. st.", 1924);
        await EnsureBook("Válka s mloky", "Karel Čapek", "Československý spisovatel", "Epika", "Román", 352, "Česká lit 20. a 21. st.", 1936);

        // Jirásek
        await EnsureBook("F.L.Věk", "Alois Jirásek", "Mladá fronta", "Epika", "Románová kronika", 648, "Sv. a česká lit 19. st.", 1888);
        await EnsureBook("Filozofská historie", "Alois Jirásek", "Mladá fronta", "Epika", "Historický román", 512, "Sv. a česká lit 19. st.", 1878);
        await EnsureBook("Temno", "Alois Jirásek", "Mladá fronta", "Epika", "Historický román", 432, "Sv. a česká lit 19. st.", 1915);
        await EnsureBook("Psohlavci", "Alois Jirásek", "Mladá fronta", "Epika", "Historický román", 384, "Sv. a česká lit 19. st.", 1884);
        await EnsureBook("Staré pověsti české", "Alois Jirásek", "Odeon", "Epika", "Soubor pověstí", 296, "Sv. a česká lit 19. st.", 1894);

        // Hrabal
        await EnsureBook("Ostře sledované vlaky", "Bohumil Hrabal", "Československý spisovatel", "Epika", "Novela", 96, "Česká lit 20. a 21. st.", 1965);
        await EnsureBook("Postřižiny", "Bohumil Hrabal", "Československý spisovatel", "Epika", "Povídka", 128, "Česká lit 20. a 21. st.", 1976);
        await EnsureBook("Obsluhoval jsem anglického krále", "Bohumil Hrabal", "Odeon", "Epika", "Román", 224, "Česká lit 20. a 21. st.", 1983);
        await EnsureBook("Taneční hodiny pro starší a pokročilé", "Bohumil Hrabal", "Československý spisovatel", "Epika", "Povídka", 144, "Česká lit 20. a 21. st.", 1964);
        await EnsureBook("Příliš hlučná samota", "Bohumil Hrabal", "Československý spisovatel", "Epika", "Novela", 104, "Česká lit 20. a 21. st.", 1976);

        // Kafka
        await EnsureBook("Proměna", "Franz Kafka", "Odeon", "Epika", "Povídka", 72, "Sv. lit 20. a 21. st.", 1915);
        await EnsureBook("Proces", "Franz Kafka", "Odeon", "Epika", "Román", 288, "Sv. lit 20. a 21. st.", 1925);
        await EnsureBook("Zámek", "Franz Kafka", "Odeon", "Epika", "Román", 352, "Sv. lit 20. a 21. st.", 1926);
        await EnsureBook("Amerika", "Franz Kafka", "Odeon", "Epika", "Román", 304, "Sv. lit 20. a 21. st.", 1927);
        await EnsureBook("V kolonii pro zločince", "Franz Kafka", "Odeon", "Epika", "Povídka", 48, "Sv. lit 20. a 21. st.", 1919);

        // Kundera
        await EnsureBook("Žert", "Milan Kundera", "Československý spisovatel", "Epika", "Román", 368, "Česká lit 20. a 21. st.", 1967);
        await EnsureBook("Nesnesitelná lehkost bytí", "Milan Kundera", "Odeon", "Epika", "Román", 320, "Česká lit 20. a 21. st.", 1984);
        await EnsureBook("Směšné lásky", "Milan Kundera", "Československý spisovatel", "Epika", "Soubor povídek", 288, "Česká lit 20. a 21. st.", 1963);
        await EnsureBook("Kniha smíchu a zapomnění", "Milan Kundera", "Odeon", "Epika", "Román", 272, "Česká lit 20. a 21. st.", 1978);
        await EnsureBook("Identity", "Milan Kundera", "Odeon", "Epika", "Novela", 168, "Česká lit 20. a 21. st.", 1998);
    }
}
