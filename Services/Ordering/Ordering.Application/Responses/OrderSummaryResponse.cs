namespace Ordering.Application.Responses;

public sealed record OrderSummaryResponse(
    int Id,
    string UserName,
    int ItemsCount,
    decimal Total
);