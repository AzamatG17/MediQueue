using MediQueue.Domain.Entities.Responses;

namespace MediQueue.Domain.DTOs.Group
{
    public record GroupForCreateDto(string GroupName, List<int> CategoryIds);
}
