using Catalog.Infrastructure.Documents;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Data;
public class CatalogContext : ICatalogContext
{
    public IMongoCollection<ProductDocument> Products { get; }
    public IMongoCollection<ProductBrandDocument> Brands { get; }
    public IMongoCollection<ProductTypeDocument> Types { get; }

    public CatalogContext(IConfiguration configuration)
    {
        var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));

        Brands = database.GetCollection<ProductBrandDocument>(configuration.GetValue<string>("DatabaseSettings:BrandsCollection"));
        Types = database.GetCollection<ProductTypeDocument>(configuration.GetValue<string>("DatabaseSettings:TypesCollection"));
        Products = database.GetCollection<ProductDocument>(configuration.GetValue<string>("DatabaseSettings:ProductsCollection"));
    }

    public async Task SeedAsync()
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        if (environment == "Development")
        {
            await BrandContextSeed.SeedDataAsync(Brands);
            await TypeContextSeed.SeedDataAsync(Types);
            await CatalogContextSeed.SeedDataAsync(Products);
        }
    }
}
