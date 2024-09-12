using MediQueue.Domain.Entities;

namespace MediQueue.Domain.DTOs.Questionnaire
{
    public record QuestionnaireForCreateDto(
        decimal? Balance, Gender? Gender, string? PassportSeria, string? PassportPinfl,
        string? PhoneNumber, string? FirstName, string? LastName, string? SurName, DateTime? DateIssue, DateTime? DateBefore,
        string? Region, string? District, string? Posolos, string? Address, DateTime? Bithdate, string? SocialSattus, string? AdvertisingChannel,
       string? HistoryDiscription, DateTime? DateCreated, bool? IsPayed, int? AccountId, List<int>? ServiceIds);
}
