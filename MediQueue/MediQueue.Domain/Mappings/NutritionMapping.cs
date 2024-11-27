using AutoMapper;
using MediQueue.Domain.DTOs.Nutrition;
using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Mappings
{
    public class NutritionMapping : Profile
    {
        public NutritionMapping()
        {
            CreateMap<Nutrition, NutritionDto>();
            CreateMap<NutritionDto, Nutrition>();
            CreateMap<NutritionForCreateDto, Nutrition>();
            CreateMap<NutritionForUpdateDto, Nutrition>();
        }
    }
}
