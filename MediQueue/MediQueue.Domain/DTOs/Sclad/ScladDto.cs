using MediQueue.Domain.DTOs.Partiya;

namespace MediQueue.Domain.DTOs.Sclad;

public record ScladDto(
    int Id, 
    string Name,
    int Branchid,
    string BranchName, 
    ICollection<PartiyaDto>? Partiyas);
