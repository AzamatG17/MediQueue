namespace MediQueue.Domain.DTOs.PaymentLekarstvo;

public record PaymentLekarstvoHelperDto(int? QuestionnaireHistoryId, List<PaymentLekarstvoForCreateDto> PaymentLekarstvoForCreateDtos);
