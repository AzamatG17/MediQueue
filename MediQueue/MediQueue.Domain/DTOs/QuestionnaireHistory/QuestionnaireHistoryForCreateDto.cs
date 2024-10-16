namespace MediQueue.Domain.DTOs.QuestionnaireHistory;

public record QuestionnaireHistoryForCreateDto(
    string? HistoryDiscription, DateTime? DateCreated, decimal? Balance, bool? IsPayed, int? AccountId, int? QuestionnaireId, List<int>? ServiceIds);