using Ordering.Core.ValueObjects;

namespace Ordering.Core.Entities;

public sealed record OrderItemSnapshot(
    int ProductId,
    string ProductName,
    Money UnitPrice,
    int Quantity,
    Money LineTotal
);
