using Ordering.Core.Common;
using Ordering.Core.Enums;
using Ordering.Core.Exceptions;
using Ordering.Core.ValueObjects;

namespace Ordering.Core.Entities;

public class Order : Entity
{
    private readonly List<OrderItem> _items = new();

    public string BuyerName { get; private set; } = string.Empty;
    public Address ShippingAddress { get; private set; } = default!;
    public Payment? Payment { get; private set; }
    public OrderStatus Status { get; private set; } = OrderStatus.Draft;

    // Expose read-only view (aggregate root controls mutations)
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    // EF Core
    private Order() { }

    // Private constructor used only by factory
    private Order(string buyerName)
    {
        BuyerName = buyerName;
    }

    private Order(string buyerName, Address shippingAddress)
    {
        if (string.IsNullOrWhiteSpace(buyerName))
        {
            throw new DomainException("Buyer name is required.");
        }

        BuyerName = buyerName.Trim();
        ShippingAddress = shippingAddress;
        Status = OrderStatus.Draft;
    }

    public static Order Create(string buyerName, Address address, Payment payment)
    {
        if (string.IsNullOrWhiteSpace(buyerName))
            throw new DomainException("Buyer name is required.");

        var order = new Order(buyerName.Trim());
        order.SetShippingAddress(address);
        order.SetPayment(payment);

        return order;
    }


    public static Order Create(string buyerName, Address shippingAddress)
        => new Order(buyerName, shippingAddress);

    public void SetShippingAddress(Address address)
    {
        if (address is null)
            throw new DomainException("Shipping address is required.");

        EnsureEditable();
        ShippingAddress = address;
    }

    public void AddItem(int productId, string productName, Money unitPrice, int quantity)
    {
        EnsureEditable();

        if (quantity <= 0)
        {
            throw new DomainException("Quantity must be greater than 0.");
        }

        // Merge same product into a single line
        var existing = _items.FirstOrDefault(i => i.ProductId == productId);

        if (existing is not null)
        {
            existing.ChangeQuantity(existing.Quantity + quantity);
            return;
        }

        _items.Add(new OrderItem(productId, productName, unitPrice, quantity));
    }

    public void ChangeItemQuantity(int productId, int newQuantity)
    {
        EnsureEditable();

        var item = _items.FirstOrDefault(i => i.ProductId == productId);

        if (item is null)
        {
            throw new DomainException("Order item not found.");
        }

        item.ChangeQuantity(newQuantity);
    }

    public void RemoveItem(int productId)
    {
        EnsureEditable();

        var item = _items.FirstOrDefault(i => i.ProductId == productId);

        if (item is null)
        {
            throw new DomainException("Order item not found.");
        }

        _items.Remove(item);
    }

    public Money Total()
    {
        return _items.Aggregate(Money.From(0), (acc, item) => acc + item.LineTotal);
    }

    public void Submit()
    {
        EnsureEditable();

        if (_items.Count == 0)
        {
            throw new DomainException("Order must have at least one item before submission.");
        }

        Status = OrderStatus.Submitted;
    }

    public void SetPayment(Payment payment)
    {
        if (Status is not OrderStatus.Submitted)
        {
            throw new DomainException("Order must be submitted before payment can be set.");
        }

        Payment = payment;
        Status = OrderStatus.Paid;
    }

    public void MarkAsShipped()
    {
        if (Status is not OrderStatus.Paid)
        {
            throw new DomainException("Order must be paid before it can be shipped.");
        }

        Status = OrderStatus.Shipped;
    }

    public void Cancel()
    {
        if (Status is OrderStatus.Shipped)
        {
            throw new DomainException("Shipped orders cannot be cancelled.");
        }

        Status = OrderStatus.Cancelled;
    }

    private void EnsureEditable()
    {
        if (Status is OrderStatus.Paid or OrderStatus.Shipped or OrderStatus.Cancelled)
        {
            throw new DomainException($"Order cannot be modified when status is {Status}.");
        }
    }
}
