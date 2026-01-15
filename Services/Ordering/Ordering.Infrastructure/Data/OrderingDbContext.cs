using Microsoft.EntityFrameworkCore;
using Ordering.Core.Common;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data;

public class OrderingDbContext : DbContext, IUnitOfWork
{
    public OrderingDbContext(DbContextOptions<OrderingDbContext> options) : base(options)
    {

    }

    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderingDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    Task IUnitOfWork.SaveChangesAsync(CancellationToken ct)
        => base.SaveChangesAsync();
}
