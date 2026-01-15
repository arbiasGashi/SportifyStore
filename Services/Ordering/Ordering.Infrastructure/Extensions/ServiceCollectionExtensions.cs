using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Persistence;
using Ordering.Infrastructure.Repositories;


namespace Ordering.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrderingDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("OrderingConnectionString"),
                sql => sql.EnableRetryOnFailure());
        });

        services.AddScoped<IOrderRepository, OrderRepository>();

        // DbContext is the Unit of Work
        services.AddScoped<Application.Abstractions.Persistence.IUnitOfWork, EfUnitOfWork>();

        return services;
    }
}
