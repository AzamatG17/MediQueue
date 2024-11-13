using MediQueue.Domain.DTOs.Benefit;
using MediQueue.Domain.DTOs.Conclusion;
using MediQueue.Domain.DTOs.Discount;
using MediQueue.Domain.DTOs.PaymentService;
using MediQueue.Domain.DTOs.ServiceUsage;

namespace MediQueue.Domain.DTOs.QuestionnaireHistory;

public record QuestionnaireHistoryDto(
    int id,
    int Historyid,
    string? HistoryDiscription,
    DateTime? DateCreated,
    decimal? Balance,
    bool? IsPayed,
    decimal? InitialDiscountPercentage,
    decimal? InitialBenefitPercentage,
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
    List<BenefitDto>? BenefitDtos,
    List<DiscountDto>? DiscountDtos,
    List<ServiceUsageDto>? ServiceUsages,
    List<PaymentServiceDto>? PaymentServices,
    List<ConclusionDto>? ConclusionDtos
    );
