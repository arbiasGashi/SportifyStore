using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Core.Entities;
using Ordering.Core.Enums;
using Ordering.Core.Repositories;
using Ordering.Core.ValueObjects;

namespace Ordering.Application.Handlers;

public sealed class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
{
    private readonly IOrderRepository _orders;
    private readonly ILogger<CheckoutOrderCommandHandler> _logger;

    public CheckoutOrderCommandHandler(
        IOrderRepository orders,
        ILogger<CheckoutOrderCommandHandler> logger)
    {
        _orders = orders;
        _logger = logger;
    }

    public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken ct)
    {
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

        // Domain-first construction (no AutoMapper into entities)
        var order = Order.Create(request.BuyerName, address, payment);

        await _orders.AddAsync(order, ct);

        _logger.LogInformation("Order created. OrderId={OrderId} BuyerName={BuyerName}", order.Id, request.BuyerName);

        return order.Id;
    }
}
