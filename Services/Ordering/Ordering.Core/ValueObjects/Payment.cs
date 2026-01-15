using Ordering.Core.Enums;
using Ordering.Core.Exceptions;

namespace Ordering.Core.ValueObjects;

public sealed record Payment(PaymentMethod Method, string PaymentReference)
{
    public static Payment Create(PaymentMethod method, string paymentReference)
    {
        if (method == PaymentMethod.Unknown)
        {
            throw new DomainException("Payment method is required");
        }

        if (string.IsNullOrWhiteSpace(paymentReference))
        {
            throw new DomainException("Payment Reference is required.");
        }

        return new Payment(method, paymentReference);
    }
}
