using MediQueue.Domain.Entities;

namespace MediQueue.Domain.DTOs.Questionnaire
{
    public record QuestionnaireForCreateDto(
        int QuestionnaireId, decimal Balance, Gender Gender, int AccountId);
}
