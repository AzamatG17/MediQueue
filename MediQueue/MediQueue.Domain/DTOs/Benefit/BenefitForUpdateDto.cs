namespace MediQueue.Domain.DTOs.Benefit;

public record BenefitForUpdateDto(
    int Id,
    string Name,
    decimal Percent
    );
