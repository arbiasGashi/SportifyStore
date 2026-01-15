using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Application.Exceptions;
using Ordering.Core.Enums;
using Ordering.Core.Repositories;
using Ordering.Core.ValueObjects;

namespace Ordering.Application.Handlers;

public sealed class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
{
    private readonly IOrderRepository _orders;
    private readonly ILogger<UpdateOrderCommandHandler> _logger;

    public UpdateOrderCommandHandler(IOrderRepository orders, ILogger<UpdateOrderCommandHandler> logger)
    {
        _orders = orders;
        _logger = logger;
    }

    public async Task Handle(UpdateOrderCommand request, CancellationToken ct)
    {
        var order = await _orders.GetByIdAsync(request.OrderId, ct);

        if (order is null)
        {
            throw new OrderNotFoundException(request.OrderId);
        }

        var address = Address.Create(
            request.ShippingAddress.FirstName,
            request.ShippingAddress.LastName,
            request.ShippingAddress.Email,
            request.ShippingAddress.AddressLine,
            request.ShippingAddress.Country,
            request.ShippingAddress.State,
            request.ShippingAddress.ZipCode);

        var payment = Payment.Create(
            (PaymentMethod)request.Payment.Method,
            request.Payment.PaymentReference);

        order.SetShippingAddress(address);
        order.SetPayment(payment);

        _logger.LogInformation("Order updated. OrderId={OrderId}", order.Id);
    }
}
