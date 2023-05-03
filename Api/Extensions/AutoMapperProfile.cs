using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.Extensions
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<IngridientComposition, GetIngridientCompositionDto>();
            CreateMap<FoodSlot, GetFoodSlotDto>()
                .ForMember(dest => dest.IngridientCompositions, opt => opt.MapFrom(src => src.IngridientCompositions));
            CreateMap<AddFoodSlotDto, FoodSlot>();
            CreateMap<AddIngridientCompositionDto, IngridientComposition>();
        }
    }
}
