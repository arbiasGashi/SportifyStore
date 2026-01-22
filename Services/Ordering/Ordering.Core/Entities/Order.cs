using Ordering.Core.Common;
using Ordering.Core.Enums;
using Ordering.Core.Exceptions;
using Ordering.Core.ValueObjects;

namespace Ordering.Core.Entities;

public class Order : Entity
{
    private const decimal TAX_RATE = 0.10m;
    private const decimal FREE_SHIPPING_THRESHOLD = 100m;
    private const decimal STANDARD_SHIPPING_COST = 10m;

    private readonly List<OrderItem> _items = new();

    public string Buyer { get; private set; } = string.Empty;
    public Address ShippingAddress { get; private set; } = default!;
    public Payment? Payment { get; private set; }
    public OrderStatus Status { get; private set; } = OrderStatus.Draft;

    // Expose read-only view (aggregate root controls mutations)
    public IReadOnlyCollection<OrderItemSnapshot> Items => _items
        .Select(item => new OrderItemSnapshot(
            item.ProductId,
            item.ProductName,
            item.UnitPrice,
            item.Quantity,
            item.LineTotal))
        .ToList();

    // EF Core
    private Order() { }

    // Private constructor used only by factory
    private Order(string buyer)
    {
        Buyer = buyer;
    }

    private Order(string buyer, Address shippingAddress)
    {
        if (string.IsNullOrWhiteSpace(buyer))
        {
            throw new DomainException("Buyer is required.");
        }

        Buyer = buyer.Trim();
        ShippingAddress = shippingAddress;
        Status = OrderStatus.Draft;
    }

    public static Order Create(string buyer, Address address, Payment payment)
    {
        if (string.IsNullOrWhiteSpace(buyer))
        {
            throw new DomainException("Buyer is required.");
        }

        var order = new Order(buyer.Trim());
        order.SetShippingAddress(address);
        order.Pay(payment);

        return order;
    }


    public static Order Create(string buyer, Address shippingAddress)
        => new Order(buyer, shippingAddress);

    public void SetShippingAddress(Address address)
    {
        if (address is null)
        {
            throw new DomainException("Shipping address is required.");
        }

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

        if (_items.Count > 0 && _items[0].UnitPrice.Currency != unitPrice.Currency)
        {
            throw new DomainException("All order items must use the same currency.");
        }

        // Merge same product into a single line
        var existing = _items.FirstOrDefault(i => i.ProductId == productId);

        if (existing is not null)
        {
            if (existing.UnitPrice.Currency != unitPrice.Currency)
            {
                throw new DomainException("Order item currency mismatch.");
            }

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

    public Money Subtotal()
    {
        if (_items.Count == 0)
        {
            return Money.From(0, Currency.USD);
        }

        var currency = _items[0].UnitPrice.Currency;
        return _items.Aggregate(Money.From(0, currency), (acc, item) => acc + item.LineTotal);
    }

    public Money Tax()
    {
        var subtotal = Subtotal();
        return Money.From(subtotal.Amount * TAX_RATE, subtotal.Currency);
    }

    public Money ShippingCost()
    {
        var subtotal = Subtotal();

        if (subtotal.Amount >= FREE_SHIPPING_THRESHOLD)
        {
            return Money.From(0, subtotal.Currency);
        }

        return Money.From(STANDARD_SHIPPING_COST, subtotal.Currency);
    }

    public Money Total()
    {
        return Subtotal() + Tax() + ShippingCost();
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

    public void Pay(Payment payment)
    {
        if (Status is not OrderStatus.Submitted)
        {
            throw new DomainException("Order must be submitted before payment can be set.");
        }

        Payment = payment;
        Status = OrderStatus.Paid;
    }

    public void Ship()
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
