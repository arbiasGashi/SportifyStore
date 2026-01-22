using MediatR;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using Ordering.Core.Repositories;

namespace Ordering.Application.QueryHandlers;

public sealed class GetOrdersByBuyerQueryHandler
    : IRequestHandler<GetOrdersByBuyerQuery, IReadOnlyList<OrderSummaryResponse>>
{
    private readonly IOrderRepository _orders;

    public GetOrdersByBuyerQueryHandler(IOrderRepository orders)
    {
        _orders = orders;
    }

    public async Task<IReadOnlyList<OrderSummaryResponse>> Handle(GetOrdersByBuyerQuery request, CancellationToken ct)
    {
        var orders = await _orders.GetByBuyerAsync(request.Buyer, ct);

        return orders
            .Select(o =>
            {
                var total = o.Total().Amount;

                return new OrderSummaryResponse(
                    Id: o.Id,
                    Buyer: o.Buyer,
                    ItemsCount: o.Items.Count,
                    Total: total
                );
            })
            .ToList();
    }
}
