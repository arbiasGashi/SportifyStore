using Catalog.Infrastructure.Documents;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data;
public static class TypeContextSeed
{
    public static async Task SeedDataAsync(IMongoCollection<ProductTypeDocument> typeCollection)
    {
        var hasTypes = await typeCollection
            .Find(t => true)
            .AnyAsync();

        if (hasTypes)
        {
            return;
        }

        // Use application base directory (safe in Docker)
        var seedPath = Path.Combine(AppContext.BaseDirectory, "Data", "SeedData", "types.json");

        if (!File.Exists(seedPath))
        {
            Console.WriteLine($"Seed file not found: {seedPath}");
            return;
        }

        Console.WriteLine($"Seeding types from: {seedPath}");
        var typesData = await File.ReadAllTextAsync(seedPath);
        var types = JsonSerializer.Deserialize<List<ProductTypeDocument>>(typesData);

        if (types != null && types.Count > 0)
        {
            await typeCollection.DeleteManyAsync(Builders<ProductTypeDocument>.Filter.Empty); // Clear before reseed
            await typeCollection.InsertManyAsync(types);
            Console.WriteLine($"Inserted {types.Count} type records.");
        }
    }
}
