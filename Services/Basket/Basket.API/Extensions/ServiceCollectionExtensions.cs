using Asp.Versioning;
using Basket.Application.Abstractions;
using Basket.Application.Commands;
using Basket.Application.Mappers;
using Basket.Application.Queries;
using Basket.Core.Repositories;
using Basket.Infrastructure.Repositories;
using Basket.Infrastructure.Services;
using Discount.Grpc.Protos;
using Microsoft.OpenApi.Models;


namespace Basket.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // AutoMapper
        services.AddAutoMapper(typeof(BasketMappingProfile).Assembly);

        // MediatR (Application layer)
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(typeof(GetBasketByUserNameQuery).Assembly,
                                               typeof(UpsertShoppingCartCommand).Assembly));

        // Redis cache (IDistributedCache)
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetSection("CacheSettings:ConnectionString").Value;
        });

        // gRPC client
        services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
        {
            options.Address = new Uri(configuration["GrpcSettings:DiscountUrl"]!);
        });

        services.AddScoped<IDiscountService, DiscountGrpcDiscountService>();

        // Repositories
        services.AddScoped<IBasketRepository, BasketRepository>();

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
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Basket.API",
                Version = "v1"
            });
        });

        return services;
    }
}
