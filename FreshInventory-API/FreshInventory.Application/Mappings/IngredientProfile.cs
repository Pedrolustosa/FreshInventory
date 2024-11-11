﻿using AutoMapper;
using FreshInventory.Domain.Entities;
using FreshInventory.Application.DTO;

namespace FreshInventory.Application.Mappings;

public class IngredientProfile : Profile
{
    public IngredientProfile()
    {
        CreateMap<Ingredient, IngredientDto>();
        CreateMap<IngredientCreateDto, Ingredient>();
        CreateMap<IngredientUpdateDto, Ingredient>()
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore());
    }
}