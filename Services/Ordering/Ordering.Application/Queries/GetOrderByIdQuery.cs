using MediatR;
using Ordering.Application.Responses;

namespace Ordering.Application.Queries;

public sealed record GetOrderByIdQuery(int OrderId)
    : IRequest<OrderResponse>;
