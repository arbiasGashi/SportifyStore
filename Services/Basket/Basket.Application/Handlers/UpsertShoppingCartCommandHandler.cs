using Basket.Application.Abstractions;
using Basket.Application.Commands;
using Basket.Application.Mappers;
using Basket.Application.Responses;
using Basket.Core.Entities;
using Basket.Core.Repositories;
using MediatR;

namespace Basket.Application.Handlers;

public class UpsertShoppingCartCommandHandler : IRequestHandler<UpsertShoppingCartCommand, ShoppingCartResponse>
{
    private readonly IBasketRepository _basketRepository;
    private readonly IDiscountService _discountService;

    public UpsertShoppingCartCommandHandler(IBasketRepository basketRepository, IDiscountService discountService)
    {
        _basketRepository = basketRepository;
        _discountService = discountService;
    }

    public async Task<ShoppingCartResponse> Handle(UpsertShoppingCartCommand request, CancellationToken cancellationToken)
    {
        foreach (var item in request.Items)
        {
            var discount = await _discountService.GetDiscountAsync(item.ProductName, cancellationToken);

            if (discount?.Amount > 0)
            {
                item.Price = Math.Max(0m, item.Price - (discount.Amount / 100m));
            }
        }

        var shoppingCart = BasketMapper.Mapper.Map<ShoppingCart>(request);
        var savedCart = await _basketRepository.UpdateBasket(shoppingCart);
        var shoppingCartResponse = BasketMapper.Mapper.Map<ShoppingCartResponse>(savedCart);

        return shoppingCartResponse;
    }
}
