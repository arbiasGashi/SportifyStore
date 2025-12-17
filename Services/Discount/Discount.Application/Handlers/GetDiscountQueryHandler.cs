using Discount.Application.Queries;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using MediatR;

namespace Discount.Application.Handlers;

public class GetDiscountQueryHandler : IRequestHandler<GetDiscountQuery, Coupon?>
{
    private readonly IDiscountRepository _discountRepository;

    public GetDiscountQueryHandler(IDiscountRepository discountRepository)
    {
        _discountRepository = discountRepository;
    }

    public async Task<Coupon?> Handle(GetDiscountQuery request, CancellationToken cancellationToken)
    {
        // Just domain logic + repository. No gRPC here.
        var coupon = await _discountRepository.GetDiscount(request.ProductName);

        return coupon; // null means "not found"
    }
}
