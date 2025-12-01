namespace Basket.Core.Entities;
public class ShoppingCart
{
    public string UserName { get; set; }
    public List<ShoppingCartItem> Items { get; set; } = new();
    public decimal TotalPrice => Items.Sum(i => i.Price * i.Quantity); // Expression-Bodied Read-Only Property

    public ShoppingCart()
    {

    }

    public ShoppingCart(string userName)
    {
        UserName = userName;
    }
}
