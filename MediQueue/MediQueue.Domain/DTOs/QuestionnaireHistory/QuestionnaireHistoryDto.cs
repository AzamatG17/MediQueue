using MediQueue.Domain.DTOs.PaymentService;
using MediQueue.Domain.DTOs.Service;
using MediQueue.Domain.Entities;

namespace MediQueue.Domain.DTOs.QuestionnaireHistory
{
    public record QuestionnaireHistoryDto(
        int id, int Historyid, string? HistoryDiscription, DateTime? DateCreated, decimal? Balance, bool? IsPayed, int? AccountId, string? AccountName, int? QuestionnaireId, List<ServiceDtos>? ServiceDtos, List<PaymentServiceDto>? PaymentServices);
}
