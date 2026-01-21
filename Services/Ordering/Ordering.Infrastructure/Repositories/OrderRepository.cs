using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;

namespace Ordering.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrderingDbContext _db;

    public OrderRepository(OrderingDbContext db)
    {
        _db = db;
    }

    public async Task<Order?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var order =  await _db
            .Orders
            .FirstOrDefaultAsync(o => o.Id == id, ct);

        return order;
    }

    public async Task<IReadOnlyList<Order>> GetByBuyerNameAsync(string buyerName, CancellationToken ct = default)
    {
        var orders = await _db
            .Orders
            .Where(o => o.BuyerName == buyerName)
            .ToListAsync(ct);

        return orders;
    }

    public async Task AddAsync(Order order, CancellationToken ct = default)
    {
        await _db
            .Orders
            .AddAsync(order, ct)
            .AsTask();
    }

    public Task DeleteAsync(Order order, CancellationToken ct = default)
    {
        if (order is null)
        {
            throw new ArgumentNullException(nameof(order));
        }

        _db.Orders.Remove(order);
        return Task.CompletedTask;
    }
}
