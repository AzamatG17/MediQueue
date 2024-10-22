using AutoMapper;
using MediQueue.Domain.DTOs.AnalysisResult;
using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Mappings
{
    public class AnalysisResultMappings : Profile
    {
        public AnalysisResultMappings()
        {
            CreateMap<AnalysisResultDto, AnalysisResult>();

            CreateMap<AnalysisResult, AnalysisResultDto>()
                .ForMember(dest => dest.ServiceId, opt => opt.MapFrom(src => src.ServiceUsage.ServiceId))
                .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.ServiceUsage.Service.Name ?? ""));

            CreateMap<AnalysisResultForCreateDto, AnalysisResult>();
            CreateMap<AnalysisResultForUpdateDto, AnalysisResult>();
        }
    }
}
