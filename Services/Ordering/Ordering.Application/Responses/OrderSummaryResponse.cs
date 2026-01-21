namespace Ordering.Application.Responses;

public sealed record OrderSummaryResponse(
    int Id,
    string BuyerName,
    int ItemsCount,
    decimal Total
);