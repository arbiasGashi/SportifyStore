using Asp.Versioning;
using Catalog.Application.Mappers;
using Catalog.Application.Queries;

namespace Catalog.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(typeof(ProductMappingProfile).Assembly);

        // MediatR
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(typeof(GetAllBrandsQuery).Assembly));

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
