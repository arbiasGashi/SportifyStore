using MediatR;
using Microsoft.Extensions.Logging;

namespace Ordering.Application.Behaviours;

public sealed class UnhandledExceptionBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<UnhandledExceptionBehaviour<TRequest, TResponse>> _logger;

    public UnhandledExceptionBehaviour(
        ILogger<UnhandledExceptionBehaviour<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;
            _logger.LogError(ex, "Unhandled exception for request {RequestName}. {@Request}", requestName, request);
            throw;
        }
    }
}
