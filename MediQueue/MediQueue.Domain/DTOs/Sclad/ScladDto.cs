using MediQueue.Domain.DTOs.Lekarstvo;

namespace MediQueue.Domain.DTOs.Sclad;

public record ScladDto(int Id, string Name, int Branchid, string BranchName, ICollection<LekarstvoDto>? Lekarstvos);
