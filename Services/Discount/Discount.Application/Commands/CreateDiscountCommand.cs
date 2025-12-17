using Discount.Core.Entities;
using MediatR;

namespace Discount.Application.Commands;

public class CreateDiscountCommand : IRequest<Coupon>
{
    public string ProductName { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int Amount { get; set; }
}