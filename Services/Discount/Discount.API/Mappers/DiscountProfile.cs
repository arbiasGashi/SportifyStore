using AutoMapper;
using Discount.Core.Entities;
using Discount.Grpc.Protos;

namespace Discount.API.Mappers;

public class DiscountProfile : Profile
{
    public DiscountProfile()
    {
        CreateMap<Coupon, CouponModel>().ReverseMap();
    }
}
