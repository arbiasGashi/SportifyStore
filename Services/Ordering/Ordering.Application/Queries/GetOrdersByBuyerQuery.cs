using MediatR;
using Ordering.Application.Responses;

namespace Ordering.Application.Queries;

public sealed record GetOrdersByBuyerQuery(string Buyer)
    : IRequest<IReadOnlyList<OrderSummaryResponse>>;
