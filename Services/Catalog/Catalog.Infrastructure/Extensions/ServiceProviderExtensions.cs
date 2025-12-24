using Catalog.Infrastructure.Data.Seeding;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Infrastructure.Extensions;

public static class ServiceProviderExtensions
{
    public static async Task EnsureCatalogDatabaseSeededAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<ICatalogDataSeeder>();
        await seeder.SeedAsync(cancellationToken);
    }
}
