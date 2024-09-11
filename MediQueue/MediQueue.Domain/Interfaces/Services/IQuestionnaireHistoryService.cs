using MediQueue.Domain.DTOs.QuestionnaireHistory;

namespace MediQueue.Domain.Interfaces.Services
{
    public interface IQuestionnaireHistoryService
    {
        Task<IEnumerable<QuestionnaireHistoryDto>> GetAllQuestionnaireHistoriessAsync();
        Task<QuestionnaireHistoryDto> GetQuestionnaireHistoryByIdAsync(int id);
        Task<QuestionnaireHistoryDto> CreateQuestionnaireHistoryAsync(QuestionnaireHistoryForCreateDto questionnaireHistoryForCreateDto);
        Task<QuestionnaireHistoryDto> UpdateQuestionnaireHistoryAsync(QuestionnaireHistoryForUpdateDto questionnaireHistoryForUpdateDto);
        Task DeleteQuestionnaireHistoryAsync(int id);
    }
}
