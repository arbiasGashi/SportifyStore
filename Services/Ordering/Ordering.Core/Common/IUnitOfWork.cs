namespace Ordering.Core.Common;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken ct = default);
}
