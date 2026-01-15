namespace Ordering.Application.Abstractions.Persistence;

public interface IUnitOfWork
{
    Task<ITransaction> BeginTransactionAsync(CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
