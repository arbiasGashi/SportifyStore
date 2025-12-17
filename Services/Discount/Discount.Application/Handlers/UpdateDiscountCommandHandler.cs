using Discount.Application.Commands;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using MediatR;

namespace Discount.Application.Handlers;

public class UpdateDiscountCommandHandler : IRequestHandler<UpdateDiscountCommand, Coupon>
{
    private readonly IDiscountRepository _discountRepository;

    public UpdateDiscountCommandHandler(IDiscountRepository discountRepository)
    {
        _discountRepository = discountRepository;
    }

    public async Task<Coupon> Handle(UpdateDiscountCommand request, CancellationToken cancellationToken)
    {
        var coupon = new Coupon
        {
            Id = request.Id,
            ProductName = request.ProductName,
            Description = request.Description,
            Amount = request.Amount
        };

        await _discountRepository.UpdateDiscount(coupon);

        return coupon;
    }
}
