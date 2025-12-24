using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Documents;
using Catalog.Infrastructure.Mappers;
using Catalog.Infrastructure.Repositories;
using Catalog.Infrastructure.Data.Seeding;
using Catalog.Infrastructure.Sorting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(ProductDocumentProfile).Assembly);

        services.AddScoped<ICatalogContext>(_ => new CatalogContext(configuration));
        services.AddScoped<ICatalogDataSeeder, CatalogDataSeeder>();

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ITypeRepository, TypeRepository>();
        services.AddScoped<IBrandRepository, BrandRepository>();

        services.AddScoped<IMongoSortStrategy<ProductDocument>, MongoNameSortStrategy>();
        services.AddScoped<IMongoSortStrategy<ProductDocument>, MongoPriceAscSortStrategy>();
        services.AddScoped<IMongoSortStrategy<ProductDocument>, MongoPriceDescSortStrategy>();
        services.AddScoped<MongoSortStrategyFactory<ProductDocument>>();

        return services;
    }
}
