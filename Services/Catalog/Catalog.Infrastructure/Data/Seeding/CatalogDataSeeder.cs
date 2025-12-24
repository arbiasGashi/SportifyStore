using Microsoft.Extensions.Hosting;

namespace Catalog.Infrastructure.Data.Seeding;

public class CatalogDataSeeder : ICatalogDataSeeder
{
    private readonly ICatalogContext _context;
    private readonly IHostEnvironment _environment;

    public CatalogDataSeeder(ICatalogContext context, IHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (!_environment.IsDevelopment())
        {
            return;
        }

        await BrandContextSeed.SeedDataAsync(_context.Brands);
        await TypeContextSeed.SeedDataAsync(_context.Types);
        await CatalogContextSeed.SeedDataAsync(_context.Products);
    }
}
