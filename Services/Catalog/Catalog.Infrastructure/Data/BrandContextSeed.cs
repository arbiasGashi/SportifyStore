using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data;
public static class BrandContextSeed
{
    public static void SeedData(IMongoCollection<ProductBrand> brandCollection)
    {
        bool checkBrands = brandCollection.Find(b => true).Any();

        // Current working directory (changes depending on environment)
        string basePath = Directory.GetCurrentDirectory();

        // Default path (Docker or root run)
        string path = Path.Combine("Data", "SeedData", "brands.json");

        if (!File.Exists(path))
        {
            path = Path.Combine(Directory.GetParent(basePath)!.FullName,
                                "Catalog.Infrastructure", "Data", "SeedData", "brands.json");
        }

        if (!checkBrands)
        {
            var brandsData = File.ReadAllText(path);
            var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

            if (brands != null)
            {
                foreach (var brand in brands)
                {
                    brandCollection.InsertOneAsync(brand);
                }
            }
        }
    }
}
