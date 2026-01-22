using Ordering.Core.Enums;
using Ordering.Core.Exceptions;

namespace Ordering.Core.ValueObjects;

public sealed record Money(decimal Amount, Currency Currency)
{
    public static Money From(decimal amount, Currency currency)
    {
        if (amount < 0)
        {
            throw new DomainException("Money cannot be negative.");
        }

        return new Money(decimal.Round(amount, 2, MidpointRounding.AwayFromZero), currency);
    }

    public static Money operator +(Money left, Money right)
    {
        if (left.Currency != right.Currency)
        {
            throw new DomainException("Cannot add money values with different currencies");
        }

        return From(left.Amount + right.Amount, left.Currency);
    }

    public static Money operator *(Money money, int quantity)
    {
        if (quantity < 0)
        {
            throw new DomainException("Quantity cannot be negative.");
        }

        return From(money.Amount * quantity, money.Currency);
    }
}
