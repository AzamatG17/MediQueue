namespace MediQueue.Domain.DTOs.Sample;

public record SampleForUpdateDto(
    int Id,
    string? Name,
    string? Description
    );