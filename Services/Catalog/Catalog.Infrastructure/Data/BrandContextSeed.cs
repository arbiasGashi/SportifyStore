using Catalog.Infrastructure.Documents;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data;
public static class BrandContextSeed
{
    public static async Task SeedDataAsync(IMongoCollection<ProductBrandDocument> brandCollection)
    {
        bool hasBrands = await brandCollection
            .Find(b => true)
            .AnyAsync();

        if (hasBrands)
        {
            return;
        }

        // Use application base directory (safe in Docker)
        var seedPath = Path.Combine(AppContext.BaseDirectory, "Data", "SeedData", "brands.json");

        if (!File.Exists(seedPath))
        {
            Console.WriteLine($"Seed file not found: {seedPath}");
            return;
        }

        Console.WriteLine($"Seeding brands from: {seedPath}");
        var brandsData = await File.ReadAllTextAsync(seedPath);
        var brands = JsonSerializer.Deserialize<List<ProductBrandDocument>>(brandsData);

        if (brands != null && brands.Count > 0)
        {
            await brandCollection.DeleteManyAsync(Builders<ProductBrandDocument>.Filter.Empty); // Clear before reseed
            await brandCollection.InsertManyAsync(brands);
            Console.WriteLine($"Inserted {brands.Count} brand records.");
        }
    }
}
