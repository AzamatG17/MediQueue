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
                .ForCtorParam(nameof(AnalysisResultDto.QuestionnaireHistoryId), opt => opt.MapFrom(src => src.QuestionnaireHistory != null ? src.QuestionnaireHistory.Historyid : 0))
                .ForCtorParam(nameof(AnalysisResultDto.FirstAccountId), opt => opt.MapFrom(src => src.FirstDoctorId))
                .ForCtorParam(nameof(AnalysisResultDto.FirstAccountName), opt => opt.MapFrom(src => src.FirstDoctor != null ? $"{src.FirstDoctor.LastName ?? ""} {src.FirstDoctor.FirstName ?? ""} {src.FirstDoctor.SurName ?? ""}" : null))
                .ForCtorParam(nameof(AnalysisResultDto.SecondAccountId), opt => opt.MapFrom(src => src.SecondDoctorId))
                .ForCtorParam(nameof(AnalysisResultDto.SecondAccountName), opt => opt.MapFrom(src => src.SecondDoctor != null ? $"{src.SecondDoctor.LastName ?? ""} {src.SecondDoctor.FirstName ?? ""} {src.SecondDoctor.SurName ?? ""}" : null));

            CreateMap<AnalysisResultForCreateDto, AnalysisResult>();
            CreateMap<AnalysisResultForUpdateDto, AnalysisResult>();
        }
    }
}
