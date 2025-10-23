using Microsoft.Extensions.DependencyInjection;

namespace Bookong.Infrastructure.Seeding;

public static class SeedServiceCollectionExtensions
{
    public static IServiceCollection AddBookongSeeders(this IServiceCollection services)
    {
        // Seeders registration will be added here as they are created
        services.AddSingleton<SeedRunner>();
        services.AddSingleton<ISeeder, Seeders.Lookup.KindSeeder>();
        services.AddSingleton<ISeeder, Seeders.Lookup.GenreSeeder>();
        services.AddSingleton<ISeeder, Seeders.Lookup.PeriodSeeder>();
        services.AddSingleton<ISeeder, Seeders.Demo.AuthorSeeder>();
        services.AddSingleton<ISeeder, Seeders.Demo.PublisherSeeder>();
        services.AddSingleton<ISeeder, Seeders.Demo.WarehouseSeeder>();
        services.AddSingleton<ISeeder, Seeders.Demo.BookSeeder>();
        return services;
    }
}
