using MediQueue.Domain.Common;

namespace MediQueue.Domain.Entities;

public class Questionnaire : EntityBase
{
    public int? QuestionnaireId { get; set; }
    public decimal? Balance { get; set; }
    public Gender? Gender { get; set; }
    public string? PassportSeria { get; set; }
    public string? PassportPinfl { get; set; }
    public string? PhoneNumber { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? SurName { get; set; }
    public DateTime? DateIssue { get; set; }
    public DateTime? DateBefore { get; set; }
    public string? Region { get; set; }
    public string? District { get; set; }
    public string? Posolos { get; set; }
    public string? Address { get; set; }
    public DateTime? Bithdate { get; set; }
    public string? SocialSattus { get; set; }
    public string? AdvertisingChannel { get; set; }
    public string? PhotoBase64 { get; set; }

    public ICollection<QuestionnaireHistory> QuestionnaireHistories { get; set; }
}
