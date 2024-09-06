using MediQueue.Domain.Entities;

namespace MediQueue.Domain.DTOs.Questionnaire
{
    public record QuestionnaireDto(
        int Id, int QuestionnaireId, decimal Balance, Gender Gender, int AccountId);
}
