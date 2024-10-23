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
                .ForCtorParam(nameof(AnalysisResultDto.Id), opt => opt.MapFrom(src => src.Id))
                .ForCtorParam(nameof(AnalysisResultDto.MeasuredValue), opt => opt.MapFrom(src => src.MeasuredValue))
                .ForCtorParam(nameof(AnalysisResultDto.Unit), opt => opt.MapFrom(src => src.Unit))
                .ForCtorParam(nameof(AnalysisResultDto.PhotoBase64), opt => opt.MapFrom(src => src.PhotoBase64))
                .ForCtorParam(nameof(AnalysisResultDto.Status), opt => opt.MapFrom(src => src.Status))
                .ForCtorParam(nameof(AnalysisResultDto.ResultDate), opt => opt.MapFrom(src => src.ResultDate))
                .ForCtorParam(nameof(AnalysisResultDto.ServiceUsageId), opt => opt.MapFrom(src => src.ServiceUsageId))
                .ForCtorParam(nameof(AnalysisResultDto.ServiceId), opt => opt.MapFrom(src => src.ServiceUsage != null ? src.ServiceUsage.ServiceId : (int?)null))
                .ForCtorParam(nameof(AnalysisResultDto.ServiceName), opt => opt.MapFrom(src => src.ServiceUsage != null ? src.ServiceUsage.Service.Name : null))
                .ForCtorParam(nameof(AnalysisResultDto.QuestionnaireHistoryId), opt => opt.MapFrom(src => src.QuestionnaireHistoryId))
                .ForCtorParam(nameof(AnalysisResultDto.AccountId), opt => opt.MapFrom(src => src.AccountId))
                .ForCtorParam(nameof(AnalysisResultDto.AccountName), opt => opt.MapFrom(src => src.Account != null ? $"{src.Account.LastName ?? ""} {src.Account.FirstName ?? ""} {src.Account.SurName ?? ""}" : null));

            CreateMap<AnalysisResultForCreateDto, AnalysisResult>();
            CreateMap<AnalysisResultForUpdateDto, AnalysisResult>();
        }
    }
}
