using Ordering.Application.Abstractions;
using Ordering.Application.Contracts;

namespace Ordering.Application.Commands;

public sealed record CheckoutOrderCommand(
    string Buyer,
    AddressDto ShippingAddress,
    PaymentDto Payment
) : ICommand<int>;
