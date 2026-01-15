namespace Ordering.Application.Exceptions;

public sealed class OrderNotFoundException : ApplicationException
{
    public OrderNotFoundException(int orderId)
        : base($"Order with id '{orderId}' was not found.")
    {
    }
}
