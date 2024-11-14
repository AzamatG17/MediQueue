using MediQueue.Domain.Entities.Responses;

namespace MediQueue.Domain.DTOs.QuestionnaireHistory;

public record QuestionnaireHistoryForCreateDto(
    string? HistoryDiscription,
    int? AccountId, 
    int QuestionnaireId,
    List<ServiceAndAccountResponse>? ServiceAndAccountIds,
    List<int>? DiscountIds,
    List<int>? BenefitIds
    );