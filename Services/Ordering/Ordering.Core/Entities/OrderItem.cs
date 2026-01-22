using Ordering.Core.Common;
using Ordering.Core.Exceptions;
using Ordering.Core.ValueObjects;
using Ordering.Core.Enums;

namespace Ordering.Core.Entities;

internal sealed class OrderItem : Entity
{
    public int ProductId { get; private set; }
    public string ProductName { get; private set; } = string.Empty;
    public Money UnitPrice { get; private set; } = Money.From(0, Currency.USD);
    public int Quantity { get; private set; }

    public Money LineTotal => UnitPrice * Quantity;


    // EF Core
    private OrderItem() { }

    internal OrderItem(int productId, string productName, Money unitPrice, int quantity)
    {
        if (productId <= 0)
        {
            throw new DomainException("ProductId must be positive.");
        }
        
        if (string.IsNullOrWhiteSpace(productName))
        {
            throw new DomainException("ProductName is required.");
        }
            
        if (quantity <= 0)
        {
            throw new DomainException("Quantity must be greater than 0.");
        }
            

        ProductId = productId;
        ProductName = productName.Trim();
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    internal void ChangeQuantity(int quantity)
    {
        if (quantity <= 0)
        {
            throw new DomainException("Quantity must be greater than 0.");
        }
            
        Quantity = quantity;
    }
}
