namespace Ordering.Application.Responses;

public sealed record OrderItemResponse(
    int ProductId,
    string ProductName,
    decimal UnitPrice,
    int Quantity,
    decimal LineTotal
);
