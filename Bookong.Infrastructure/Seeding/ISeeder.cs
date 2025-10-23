using System.Threading;
using System.Threading.Tasks;
using Bookong.Infrastructure.Data;

namespace Bookong.Infrastructure.Seeding;

public interface ISeeder
{
    // Descriptive name for logs
    string Name { get; }

    // Ordering within the pipeline (lower runs first)
    int Order { get; }

    // True = only run when demo data is requested
    bool IsDemoData { get; }

    Task SeedAsync(BookongDbContext db, CancellationToken cancellationToken = default);
}
