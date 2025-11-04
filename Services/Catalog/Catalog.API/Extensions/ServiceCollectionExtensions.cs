using Asp.Versioning;
using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Documents;
using Catalog.Infrastructure.Mappers;
using Catalog.Infrastructure.Repositories;
using Catalog.Infrastructure.Sorting;

namespace Catalog.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(typeof(ProductMappingProfile).Assembly, typeof(ProductDocumentProfile).Assembly);

        // MediatR
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(typeof(GetAllBrandsQuery).Assembly));

        // Context and Repositories
        services.AddScoped<ICatalogContext, CatalogContext>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ITypeRepository, TypeRepository>();
        services.AddScoped<IBrandRepository, BrandRepository>();

        // Sorting Strategies and Factory
        services.AddScoped<IMongoSortStrategy<ProductDocument>, MongoNameSortStrategy>();
        services.AddScoped<IMongoSortStrategy<ProductDocument>, MongoPriceAscSortStrategy>();
        services.AddScoped<IMongoSortStrategy<ProductDocument>, MongoPriceDescSortStrategy>();
        services.AddScoped<MongoSortStrategyFactory<ProductDocument>>();

        return services;
    }

    public static IServiceCollection AddApiVersioningConfig(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
        });

        return services;
    }

    public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Catalog.API",
                Version = "v1"
            });
        });

        return services;
    }
}
