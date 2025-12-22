using Basket.Application.Abstractions;
using Discount.Grpc.Protos;

namespace Basket.Infrastructure.Services;

public sealed class DiscountGrpcDiscountService : IDiscountService
{
    private readonly DiscountProtoService.DiscountProtoServiceClient _client;

    public DiscountGrpcDiscountService(DiscountProtoService.DiscountProtoServiceClient client)
    {
        _client = client;
    }

    public async Task<DiscountResult?> GetDiscountAsync(string productName, CancellationToken cancellationToken = default)
    {
        var response = await _client.GetDiscountAsync(
            new GetDiscountRequest { ProductName = productName },
            cancellationToken: cancellationToken);

        // Discount service returns "No Discount" instead of null
        if (response is null || response.Amount <= 0)
        {
            return new DiscountResult(productName, 0, string.Empty);
        }

        return new DiscountResult(response.ProductName, response.Amount, response.Description);
    }
}
