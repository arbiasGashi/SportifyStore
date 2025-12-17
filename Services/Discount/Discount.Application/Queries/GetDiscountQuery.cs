using MediatR;
using Discount.Core.Entities;


namespace Discount.Application.Queries;

public class GetDiscountQuery : IRequest<Coupon?>
{
    public string ProductName { get; set; }

    public GetDiscountQuery(string productName)
    {
        ProductName = productName;
    }
}
