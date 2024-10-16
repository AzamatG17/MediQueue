namespace MediQueue.Domain.DTOs.PaymentService;

public record PaymentServiceHelperDto(
    int? QuestionnaireHistoryId, 
    List<PaymentServiceForCreateDto> PaymentServiceForCreateDtos);
