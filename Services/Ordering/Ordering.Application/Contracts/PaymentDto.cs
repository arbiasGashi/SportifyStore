namespace Ordering.Application.Contracts;

public sealed record PaymentDto(
    int Method,
    string PaymentReference);
