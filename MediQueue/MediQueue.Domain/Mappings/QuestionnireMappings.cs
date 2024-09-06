using AutoMapper;
using MediQueue.Domain.DTOs.Questionnaire;
using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Mappings
{
    public class QuestionnireMappings : Profile
    {
        public QuestionnireMappings()
        {
            CreateMap<QuestionnaireDto, Questionnaire>();
            CreateMap<Questionnaire, QuestionnaireDto>();
            CreateMap<QuestionnaireForCreateDto, Questionnaire>();
            CreateMap<QuestionnaireForUpdateDto, Questionnaire>();
        }
    }
}
