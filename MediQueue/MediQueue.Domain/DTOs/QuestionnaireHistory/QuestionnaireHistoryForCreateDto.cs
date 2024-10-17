namespace MediQueue.Domain.DTOs.QuestionnaireHistory;

public record QuestionnaireHistoryForCreateDto(
    string? HistoryDiscription, int? AccountId, int? QuestionnaireId, List<int>? ServiceIds);