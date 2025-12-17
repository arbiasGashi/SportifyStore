using Discount.Application.Handlers;
using Discount.Core.Repositories;
using Discount.Infrastructure.Repositories;

namespace Discount.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // AutoMapper (API layer mappings)
        services.AddAutoMapper(typeof(Program).Assembly);

        // MediatR (Application layer)
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(
                typeof(CreateDiscountCommandHandler).Assembly
            ));

        // Repositories
        services.AddScoped<IDiscountRepository, DiscountRepository>();

        return services;
    }

    public static IServiceCollection AddGrpcServices(this IServiceCollection services)
    {
        services.AddGrpc();

        return services;
    }
}
