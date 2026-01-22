namespace Ordering.Application.Responses;

public sealed record OrderSummaryResponse(
    int Id,
    string Buyer,
    int ItemsCount,
    decimal Total
);