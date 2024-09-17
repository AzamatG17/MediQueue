using MediQueue.Domain.DTOs.Sclad;

namespace MediQueue.Domain.DTOs.Branch;

public record BranchDto(int Id, string Name, string? Addres, ICollection<ScladDto>? Sclads);
