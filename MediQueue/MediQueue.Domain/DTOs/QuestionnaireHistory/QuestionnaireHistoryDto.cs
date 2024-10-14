using MediQueue.Domain.DTOs.Conclusion;
using MediQueue.Domain.DTOs.PaymentLekarstvo;
using MediQueue.Domain.DTOs.PaymentService;
using MediQueue.Domain.DTOs.Service;

namespace MediQueue.Domain.DTOs.QuestionnaireHistory;

public record QuestionnaireHistoryDto(
    int id,
    int Historyid,
    string? HistoryDiscription,
    DateTime? DateCreated,
    decimal? Balance,
    bool? IsPayed,
    int? AccountId,
    string? AccountName,
    int? QuestionnaireId,
    int? QuestionnaireAnketaId,
    string? PassportPinfl, 
    string? PassportSeria, 
    string? PhoneNumber,
    string? QuestionnaireName, 
    DateTime? Bithdate, 
    string? PhotoBase64,
    List<ServiceDtos>? ServiceDtos,
    List<PaymentServiceDto>? PaymentServices,
    List<ConclusionDto>? ConclusionDtos,
    List<PaymentLekarstvoDto>? PaymentLekarstvoDtos
    );
