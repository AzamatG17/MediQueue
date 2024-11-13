
namespace MediQueue.Domain.DTOs.QuestionnaireHistory
{
    public record QuestionnaireHistoryForUpdateDto(
        int id,
        int? Historyid,
        string? HistoryDiscription,
        DateTime? DateCreated, 
        decimal? Balance,
        bool? IsPayed,
        int? AccountId, 
        int? QuestionnaireId,
        List<int>? ServiceIds,
        List<int>? DiscountIds,
        List<int>? BenefitIds
        );
}