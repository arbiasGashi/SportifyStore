using Catalog.Infrastructure.Documents;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data;
public static class CatalogContextSeed
{
    public static async Task SeedDataAsync(IMongoCollection<ProductDocument> productCollection)
    {
        var hasProducts = await productCollection
            .Find(p => true)
            .AnyAsync();

        if (hasProducts)
        {
            return;
        }

        // Use application base directory (safe in Docker)
        var seedPath = Path.Combine(AppContext.BaseDirectory, "Data", "SeedData", "products.json");

        if (!File.Exists(seedPath))
        {
            Console.WriteLine($"Seed file not found: {seedPath}");
            return;
        }

        Console.WriteLine($"Seeding products from: {seedPath}");
        var productsData = await File.ReadAllTextAsync(seedPath);
        var products = JsonSerializer.Deserialize<List<ProductDocument>>(productsData);

        if (products != null && products.Count > 0)
        {
            await productCollection.DeleteManyAsync(Builders<ProductDocument>.Filter.Empty); // Clear before reseed
            await productCollection.InsertManyAsync(products);
            Console.WriteLine($"Inserted {products.Count} product records.");
        }
    }
}
