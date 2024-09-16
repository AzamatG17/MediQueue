using MediQueue.Domain.DTOs.QuestionnaireHistory;
using MediQueue.Domain.Entities;

namespace MediQueue.Domain.DTOs.Questionnaire
{
    public record QuestionnaireDto(
        int? Id, int? QuestionnaireId, decimal? Balance, Gender? Gender, string? PassportSeria, string? PassportPinfl,
        string? PhoneNumber, string? FirstName, string? LastName, string? SurName, DateTime? DateIssue, DateTime? DateBefore,
        string? Region, string? District, string? Posolos, string? Address, DateTime? Bithdate, string? SocialSattus, string? AdvertisingChannel, string? PhotoBase64, List<QuestionnaireHistoryWithServiceDto>? Questionnaires);
}