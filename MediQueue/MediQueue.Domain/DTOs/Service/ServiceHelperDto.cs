namespace MediQueue.Domain.DTOs.Service;

public record ServiceHelperDto(
    int id,
    string Name,
    decimal Amount,
    int CategoryId,
    string CategoryName
    );