using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Infrastructure.Data;

namespace Ordering.Infrastructure.Extensions;

public static class ServiceProviderExtensions
{
    public static async Task ApplyOrderingMigrationAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OrderingDbContext>();

        await dbContext.Database.MigrateAsync(cancellationToken);
    }
}
