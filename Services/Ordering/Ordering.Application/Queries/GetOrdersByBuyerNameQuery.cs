using MediatR;
using Ordering.Application.Responses;

namespace Ordering.Application.Queries;

public sealed record GetOrdersByBuyerNameQuery(string BuyerName)
    : IRequest<IReadOnlyList<OrderSummaryResponse>>;
