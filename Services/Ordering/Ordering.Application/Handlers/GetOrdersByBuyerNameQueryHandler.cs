using MediatR;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using Ordering.Core.Repositories;

namespace Ordering.Application.QueryHandlers;

public sealed class GetOrdersByBuyerNameQueryHandler
    : IRequestHandler<GetOrdersByBuyerNameQuery, IReadOnlyList<OrderSummaryResponse>>
{
    private readonly IOrderRepository _orders;

    public GetOrdersByBuyerNameQueryHandler(IOrderRepository orders)
    {
        _orders = orders;
    }

    public async Task<IReadOnlyList<OrderSummaryResponse>> Handle(GetOrdersByBuyerNameQuery request, CancellationToken ct)
    {
        var orders = await _orders.GetByBuyerNameAsync(request.BuyerName, ct);

        return orders
            .Select(o =>
            {
                var total = o.Items.Sum(i => i.LineTotal.Amount); // uses domain behavior
                return new OrderSummaryResponse(
                    Id: o.Id,
                    BuyerName: o.BuyerName,
                    ItemsCount: o.Items.Count,
                    Total: total
                );
            })
            .ToList();
    }
}
