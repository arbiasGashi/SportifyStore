namespace Ordering.Application.Responses;

public sealed record OrderResponse(
    int Id,
    string BuyerName,
    AddressResponse ShippingAddress,
    PaymentResponse Payment,
    IReadOnlyList<OrderItemResponse> Items,
    decimal Total
);

public sealed record AddressResponse(
    string FirstName,
    string LastName,
    string Email,
    string AddressLine,
    string Country,
    string State,
    string ZipCode
);

public sealed record PaymentResponse(
    int Method,
    string PaymentReference
);
