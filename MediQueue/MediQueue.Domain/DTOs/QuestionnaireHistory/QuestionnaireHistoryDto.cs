using MediQueue.Domain.DTOs.PaymentService;
using MediQueue.Domain.DTOs.Service;

namespace MediQueue.Domain.DTOs.QuestionnaireHistory;

public record QuestionnaireHistoryDto(
    int id, int Historyid, string? HistoryDiscription, DateTime? DateCreated, decimal? Balance, bool? IsPayed, int? AccountId, string? AccountName, int? QuestionnaireId, string? PassportPinfl, string? PhoneNumber, List<ServiceDtos>? ServiceDtos, List<PaymentServiceDto>? PaymentServices);
