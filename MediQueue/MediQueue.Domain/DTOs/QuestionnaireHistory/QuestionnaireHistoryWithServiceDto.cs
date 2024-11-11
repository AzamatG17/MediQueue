using MediQueue.Domain.DTOs.ServiceUsage;

namespace MediQueue.Domain.DTOs.QuestionnaireHistory
{
    public record QuestionnaireHistoryWithServiceDto(
        int id, int Historyid, string? HistoryDiscription, DateTime? DateCreated, decimal? Balance, bool? IsPayed, int? AccountId, string? AccountName, int? QuestionnaireId, List<ServiceUsageDto>? ServiceDtos);
}
