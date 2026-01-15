using Ordering.Application.Abstractions.Persistence;
using Ordering.Infrastructure.Data;

namespace Ordering.Infrastructure.Persistence;

public sealed class EfUnitOfWork : IUnitOfWork
{
    private readonly OrderingDbContext _db;

    public EfUnitOfWork(OrderingDbContext db)
    {
        _db = db;
    }

    public async Task<ITransaction> BeginTransactionAsync(CancellationToken ct = default)
    {
        // If you want to avoid nested tx, you can reuse existing:
        if (_db.Database.CurrentTransaction is not null)
        {
            return new NoOpTransaction();
        }

        var efTx = await _db.Database.BeginTransactionAsync(ct);
        return new EfTransaction(efTx);
    }

    public Task SaveChangesAsync(CancellationToken ct = default)
        => _db.SaveChangesAsync(ct);

    private sealed class NoOpTransaction : ITransaction
    {
        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
        public Task CommitAsync(CancellationToken ct = default) => Task.CompletedTask;
        public Task RollbackAsync(CancellationToken ct = default) => Task.CompletedTask;
    }
}
