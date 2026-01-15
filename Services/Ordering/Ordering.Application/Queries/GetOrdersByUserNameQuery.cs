using MediatR;
using Ordering.Application.Responses;

namespace Ordering.Application.Queries;

public sealed record GetOrdersByUserNameQuery(string UserName)
    : IRequest<IReadOnlyList<OrderSummaryResponse>>;
