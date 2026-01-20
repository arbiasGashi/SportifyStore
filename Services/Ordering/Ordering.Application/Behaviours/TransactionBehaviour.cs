using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Abstractions;
using Ordering.Application.Abstractions.Persistence;

namespace Ordering.Application.Behaviours;

public sealed class TransactionBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IUnitOfWork _uow;
    private readonly ILogger<TransactionBehaviour<TRequest, TResponse>> _logger;

    public TransactionBehaviour(
        IUnitOfWork uow,
        ILogger<TransactionBehaviour<TRequest, TResponse>> logger)
    {
        _uow = uow;
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        if (request is not ICommandMarker)
        {
            // It's a query, no transaction needed
            return await next();
        }

        var requestName = typeof(TRequest).Name;

        await using var tx = await _uow.BeginTransactionAsync(ct);

        try
        {
            _logger.LogDebug("BEGIN TRANSACTION for {RequestName}", requestName);

            var response = await next();

            await _uow.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);

            _logger.LogDebug("COMMIT TRANSACTION for {RequestName}", requestName);

            return response;
        }
        catch
        {
            _logger.LogWarning("ROLLBACK TRANSACTION for {RequestName}", requestName);
            await tx.RollbackAsync(ct);
            throw;
        }
    }
}
