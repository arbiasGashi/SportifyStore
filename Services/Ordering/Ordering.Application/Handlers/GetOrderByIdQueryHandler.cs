using MediatR;
using Ordering.Application.Exceptions;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using Ordering.Core.Repositories;

namespace Ordering.Application.QueryHandlers;

public sealed class GetOrderByIdQueryHandler
    : IRequestHandler<GetOrderByIdQuery, OrderResponse>
{
    private readonly IOrderRepository _orders;

    public GetOrderByIdQueryHandler(IOrderRepository orders)
    {
        _orders = orders;
    }

    public async Task<OrderResponse> Handle(GetOrderByIdQuery request, CancellationToken ct)
    {
        var order = await _orders.GetByIdAsync(request.OrderId, ct);

        if (order is null)
        {
            throw new OrderNotFoundException(request.OrderId);
        }
            
        var items = order.Items
            .Select(i => new OrderItemResponse(
                ProductId: i.ProductId,
                ProductName: i.ProductName,
                UnitPrice: i.UnitPrice.Amount,
                Quantity: i.Quantity,
                LineTotal: i.LineTotal.Amount
            ))
            .ToList();

        var address = new AddressResponse(
            order.ShippingAddress.FirstName,
            order.ShippingAddress.LastName,
            order.ShippingAddress.Email,          // if your Address uses `email` lower-case
            order.ShippingAddress.AddressLine,
            order.ShippingAddress.Country,
            order.ShippingAddress.State,
            order.ShippingAddress.ZipCode
        );

        var payment = new PaymentResponse(
            Method: (int)order.Payment.Method,
            PaymentReference: order.Payment.PaymentReference
        );

        var subtotal = order.Subtotal();
        var tax = order.Tax();
        var shippingCost = order.ShippingCost();
        var total = order.Total();

        return new OrderResponse(
            Id: order.Id,
            Buyer: order.Buyer,
            ShippingAddress: address,
            Payment: payment,
            Items: items,
            Subtotal: subtotal.Amount,
            Tax: tax.Amount,
            ShippingCost: shippingCost.Amount,
            Total: total.Amount
        );
    }
}
