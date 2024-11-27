using AutoMapper;
using MediQueue.Domain.DTOs.Tariff;
using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Mappings
{
    public class TariffMapping : Profile
    {
        public TariffMapping()
        {
            CreateMap<Tariff, TariffDto>();
            CreateMap<TariffDto, Tariff>();
            CreateMap<TariffForCreateDto, Tariff>();
            CreateMap<TariffForUpdateDto, Tariff>();
        }
    }
}
