using MediQueue.Domain.DTOs.Questionnaire;

namespace MediQueue.Domain.Interfaces.Services
{
    public interface IQuestionnaireService
    {
        Task<IEnumerable<QuestionnaireDto>> GetAllQuestionnairesAsync();
        Task<QuestionnaireDto> GetQuestionnaireByIdAsync(int id);
        Task<QuestionnaireDto> CreateQuestionnaireAsync(QuestionnaireForCreateDto questionnaireForCreateDto);
        Task<QuestionnaireDto> CreateOrGetBId(QuestionnaireForCreateDto questionnaireForCreateDto);
        Task<QuestionnaireDto> UpdateQuestionnaireAsync(QuestionnaireForUpdateDto questionnaireForUpdateDto);
        Task DeleteQuestionnaireAsync(int id);
    }
}
