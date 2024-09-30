using MediQueue.Domain.DTOs.Questionnaire;
using MediQueue.Domain.ResourceParameters;

namespace MediQueue.Domain.Interfaces.Services
{
    public interface IQuestionnaireService
    {
        Task<IEnumerable<QuestionnaireDto>> GetAllQuestionnairesAsync(QuestionnaireResourceParameters questionnaireResourceParameters);
        Task<QuestionnaireDto> GetQuestionnaireByIdAsync(int id);
        Task<QuestionnaireDto> CreateQuestionnaireAsync(QuestionnaireForCreateDto questionnaireForCreateDto);
        Task<QuestionnaireDto> CreateOrGetBId(QuestionnaireForCreateDto questionnaireForCreateDto);
        Task<QuestionnaireDto> UpdateQuestionnaireAsync(QuestionnaireForUpdateDto questionnaireForUpdateDto);
        Task DeleteQuestionnaireAsync(int id);
    }
}
