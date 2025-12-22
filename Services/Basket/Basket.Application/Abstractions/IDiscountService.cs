namespace Basket.Application.Abstractions;

public interface IDiscountService
{
    Task<DiscountResult?> GetDiscountAsync(string productName, CancellationToken cancellationToken = default);
}
