using MediatR;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using Ordering.Core.Repositories;

namespace Ordering.Application.QueryHandlers;

public sealed class GetOrdersByUserNameQueryHandler
    : IRequestHandler<GetOrdersByUserNameQuery, IReadOnlyList<OrderSummaryResponse>>
{
    private readonly IOrderRepository _orders;

    public GetOrdersByUserNameQueryHandler(IOrderRepository orders)
    {
        _orders = orders;
    }

    public async Task<IReadOnlyList<OrderSummaryResponse>> Handle(GetOrdersByUserNameQuery request, CancellationToken ct)
    {
        var orders = await _orders.GetByUserNameAsync(request.UserName, ct);

        return orders
            .Select(o =>
            {
                var total = o.Items.Sum(i => i.LineTotal.Amount); // uses domain behavior
                return new OrderSummaryResponse(
                    Id: o.Id,
                    UserName: o.UserName,
                    ItemsCount: o.Items.Count,
                    Total: total
                );
            })
            .ToList();
    }
}
