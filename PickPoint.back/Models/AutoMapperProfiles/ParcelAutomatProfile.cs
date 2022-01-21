using AutoMapper;
using PickPoint.back.Models.ParcelAutomatModel;
using PickPoint.back.Models.ParcelAutomatModel.ForOnlineStore;

namespace PickPoint.back.Models.AutoMapperProfiles;

public class ParcelAutomatProfile : Profile
{
  public ParcelAutomatProfile()
  {
    CreateMap<ParcelAutomat, ParcelAutomatOnlineStoreReadDto>();
  }
}