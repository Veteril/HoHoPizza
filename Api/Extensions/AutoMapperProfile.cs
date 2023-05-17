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
            CreateMap<User, UserDto>();
            CreateMap<FoodSlot, FoodSlotInOrderDto>();
            CreateMap<ShoppingCart, ShoppingCartDto>()
                .ForMember(dest => dest.FoodSlot, opt => opt.MapFrom(src => src.FoodSlot));
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.ShoppingCarts, opt => opt.MapFrom(src => src.ShoppingCarts));
        }
    }
}
