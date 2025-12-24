namespace Catalog.Infrastructure.Data.Seeding;

public interface ICatalogDataSeeder
{
    Task SeedAsync(CancellationToken cancellationToken = default);
}
