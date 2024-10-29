using MediQueue.Domain.DTOs.ScladLekarstvo;

namespace MediQueue.Domain.DTOs.Sclad;

public record ScladDto(
    int Id, 
    string Name,
    int Branchid,
    string BranchName, 
    ICollection<ScladLekarstvoDto>? Lekarstvos);
