using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bookong.Domain.Entities;
using Bookong.Infrastructure.Data;

namespace Bookong.Infrastructure.Seeding.Seeders.Demo;

public class WarehouseSeeder : ISeeder
{
    public string Name => "Demo Warehouses";
    public int Order => 210;
    public bool IsDemoData => true;

    public async Task SeedAsync(BookongDbContext db, CancellationToken cancellationToken = default)
    {
        var address = db.Addresses.FirstOrDefault(a => a.Street == "Hlavní" && a.City == "Trutnov");
        if (address is null)
        {
            address = new Address
            {
                PublicId = Guid.NewGuid(),
                Street = "Hlavní",
                City = "Trutnov",
                Number = "1",
                ZipCode = "541 01"
            };
            db.Addresses.Add(address);
            await db.SaveChangesAsync(cancellationToken);
        }

        if (!db.Warehouses.Any(w => w.Name == "Sklad A"))
        {
            db.Warehouses.Add(new Warehouse
            {
                PublicId = Guid.NewGuid(),
                Name = "Sklad A",
                Address = address
            });
        }

        await db.SaveChangesAsync(cancellationToken);
    }
}
