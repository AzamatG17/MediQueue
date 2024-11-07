using AutoMapper;
using MediQueue.Domain.DTOs.Sample;
using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Mappings
{
    public class SampleMappings : Profile
    {
        public SampleMappings()
        {
            CreateMap<SampleDto, Sample>();
            CreateMap<Sample, SampleDto>();
            CreateMap<SampleForCreateDto, Sample>();
            CreateMap<SampleForUpdateDto, Sample>();
        }
    }
}
