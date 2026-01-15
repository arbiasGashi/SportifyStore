using Ordering.Core.Exceptions;

namespace Ordering.Core.ValueObjects;

public sealed record Money(decimal Amount)
{
    public static Money From(decimal amount)
    {
        if (amount < 0)
        {
            throw new DomainException("Money cannot be negative.");
        }

        return new Money(decimal.Round(amount, 2, MidpointRounding.AwayFromZero));
    }

    public static Money operator +(Money left, Money right)
        => From(left.Amount + right.Amount);

    public static Money operator *(Money money, int quantity)
    {
        if (quantity < 0)
        {
            throw new DomainException("Quantity cannot be negative.");
        }

        return From(money.Amount * quantity);
    }
}
