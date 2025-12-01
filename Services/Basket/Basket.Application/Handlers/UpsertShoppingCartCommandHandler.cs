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

    public UpsertShoppingCartCommandHandler(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
    }

    public async Task<ShoppingCartResponse> Handle(UpsertShoppingCartCommand request, CancellationToken cancellationToken)
    {
        // TODO: Will be integrating Discount Service`

        var shoppingCart = BasketMapper.Mapper.Map<ShoppingCart>(request);
        var savedCart = await _basketRepository.UpdateBasket(shoppingCart);
        var shoppingCartResponse = BasketMapper.Mapper.Map<ShoppingCartResponse>(shoppingCart);

        return shoppingCartResponse;
    }
}
