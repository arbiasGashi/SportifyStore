using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data;
public static class TypeContextSeed
{
    public static void SeedData(IMongoCollection<ProductType> typeCollection)
    {
        bool checkTypes = typeCollection.Find(b => true).Any();

        // Current working directory (changes depending on environment)
        string basePath = Directory.GetCurrentDirectory();

        // Default path (Docker or root run)
        string path = Path.Combine("Data", "SeedData", "types.json");

        if (!File.Exists(path))
        {
            path = Path.Combine(Directory.GetParent(basePath)!.FullName,
                                "Catalog.Infrastructure", "Data", "SeedData", "types.json");
        }

        if (!checkTypes)
        {
            var typesData = File.ReadAllText(path);
            var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

            if (types != null)
            {
                foreach (var type in types)
                {
                    typeCollection.InsertOneAsync(type);
                }
            }
        }
    }
}
