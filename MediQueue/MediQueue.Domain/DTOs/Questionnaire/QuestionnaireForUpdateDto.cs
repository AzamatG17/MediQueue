using MediQueue.Domain.Entities;

namespace MediQueue.Domain.DTOs.Questionnaire
{
    public record QuestionnaireForUpdateDto(
        int Id, int QuestionnaireId, decimal Balance, Gender Gender, int AccountId);
}
