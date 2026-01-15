using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Application.Exceptions;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers;

public sealed class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
{
    private readonly IOrderRepository _orders;
    private readonly ILogger<DeleteOrderCommandHandler> _logger;

    public DeleteOrderCommandHandler(IOrderRepository orders, ILogger<DeleteOrderCommandHandler> logger)
    {
        _orders = orders;
        _logger = logger;
    }

    public async Task Handle(DeleteOrderCommand request, CancellationToken ct)
    {
        var order = await _orders.GetByIdAsync(request.OrderId, ct);

        if (order is null)
        {
            throw new OrderNotFoundException(request.OrderId);
        }

        await _orders.DeleteAsync(order, ct);

        _logger.LogInformation("Order deleted. OrderId={OrderId}", order.Id);
    }
}
