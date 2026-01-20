using Ordering.Core.Entities;

namespace Ordering.Core.Repositories;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<Order>> GetByUserNameAsync(string userName, CancellationToken ct = default);
    Task AddAsync(Order order, CancellationToken ct = default);
    Task DeleteAsync(Order order, CancellationToken ct = default);
}
