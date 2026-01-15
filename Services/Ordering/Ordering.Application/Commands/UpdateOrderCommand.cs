using Ordering.Application.Abstractions;
using Ordering.Application.Contracts;

namespace Ordering.Application.Commands;

public sealed record UpdateOrderCommand(
    int OrderId,
    AddressDto ShippingAddress,
    PaymentDto Payment
) : ICommand;
