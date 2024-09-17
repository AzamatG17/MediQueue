using MediQueue.Domain.DTOs.Lekarstvo;

namespace MediQueue.Domain.DTOs.CategoryLekarstvo;

public record CategoryLekarstvoDto(int Id, string Name, ICollection<LekarstvoDto>? Lekarstvos);
