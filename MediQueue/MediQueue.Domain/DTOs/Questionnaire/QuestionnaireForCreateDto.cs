using MediQueue.Domain.Entities;

namespace MediQueue.Domain.DTOs.Questionnaire
{
    public record QuestionnaireForCreateDto(
        int QuestionnaireId, decimal Balance, Gender Gender, string Passport, string PhoneNumber, string FirstName, string LastName, string SurName, string Region, DateTime Bithdate, string? HistoryDiscription);
}
