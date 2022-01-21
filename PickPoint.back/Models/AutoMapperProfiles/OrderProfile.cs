using AutoMapper;
using PickPoint.back.Models.GoodModel;
using PickPoint.back.Models.OrderModel;
using PickPoint.back.Models.OrderModel.ForOnlineStore;
using System.Linq;

namespace PickPoint.back.Models.AutoMapperProfiles;

public class OrderProfile : Profile
{
  public OrderProfile()
  {
    CreateMap<Order, OrderOnlineStoreReadDTO>()
      .ForMember(dest => dest.ParcelAutomatIndex, o => o.MapFrom(src => src.ParcelAutomat.Index))
      .ForMember(dest => dest.Goods, o => o.MapFrom(src => src.Goods.Select(g => g.Name)));
    CreateMap<OrderOnlineStoreCreateDto, Order>()
      .ForMember(dest => dest.Goods, o => o.MapFrom(src => src.Goods.Select(g => new Good(g))));
    CreateMap<OrderOnlineStoreUpdateDto, Order>()
      .ForMember(dest => dest.Goods, o => o.MapFrom(src => src.Goods.Select(g => new Good(g))));
    CreateMap<Order, OrderOnlineStoreUpdateDto>();
  }
}