
using MediQueue.Domain.Entities.Responses;

namespace MediQueue.Domain.DTOs.QuestionnaireHistory
{
    public record QuestionnaireHistoryForUpdateDto(
        int Id,
        int? Historyid,
        string? HistoryDiscription,
        DateTime? DateCreated, 
        decimal? Balance,
        bool? IsPayed,
        int? AccountId, 
        int? QuestionnaireId,
        List<ServiceAndAccountResponse>? ServiceAndAccountIds,
        List<int>? DiscountIds,
        List<int>? BenefitIds
        );
}