using AutoMapper;
using FreshInventory.Domain.Entities;
using FreshInventory.Application.DTO;

namespace FreshInventory.Application.Mappings
{
    public class IngredientProfile : Profile
    {
        public IngredientProfile()
        {
            CreateMap<Ingredient, IngredientDto>().ReverseMap();
        }
    }
}
